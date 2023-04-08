using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.Struments.CoretorOrtografic.Core.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker
{
    public class FurlanSpellChecker : ISpellChecker
    {
        private readonly IKeyValueDatabase _keyValueDatabase;

        private ICollection<IProcessedElement> _processedElements = new List<IProcessedElement>();

        public IReadOnlyCollection<IProcessedElement> ProcessedElements
        {
            get
            {
                return _processedElements.ToList().AsReadOnly();
            }
        }
        public IReadOnlyCollection<IProcessedElement> ProcessedWords
        {
            get
            {
                return _processedElements.Where(element => element.GetType() == typeof(ProcessedWord)).ToList().AsReadOnly();
            }
        }

        public FurlanSpellChecker(IKeyValueDatabase keyValueDatabase)
        {
            _keyValueDatabase = keyValueDatabase;
        }


        public void ExecuteSpellCheck(string text)
        {
            _processedElements = ProcessText(text);
            foreach (ProcessedWord word in ProcessedWords)
            {
                word.Correct = CheckWord(word).Result;
            }
        }
        public void CleanSpellChecker()
        {
            _processedElements = new List<IProcessedElement>();
        }

        public async Task<bool> CheckWord(ProcessedWord word)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                string wordAsString = word.Original;
                string lcWord = wordAsString.ToLower();

                if (Regex.IsMatch(wordAsString, @"\d|^[^" + FriulianConstants.WORD_LETTERS + "]+$"))
                {
                    return true;
                }

                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(lcWord);

                var retrievedValue1 = _keyValueDatabase.GetValueAsStringByKey(wordHashes.Item1);
                var retrievedValue2 = _keyValueDatabase.GetValueAsStringByKey(wordHashes.Item2);
                List<string> suggestedWords = new();

                if (retrievedValue1 is not null && retrievedValue1 != String.Empty)
                {
                    suggestedWords.AddRange(retrievedValue1.Split(','));
                }
                if (retrievedValue2 is not null && retrievedValue2 != String.Empty)
                {
                    suggestedWords.AddRange(retrievedValue2.Split(','));
                }

                bool wordFoundInSuggestions = suggestedWords.Any(s => s.ToLower() == lcWord);

                if (wordFoundInSuggestions)
                {
                    return true;
                }
                else if (lcWord.Length > 2 && lcWord.StartsWith("l'"))
                {
                    ProcessedWord dx = new ProcessedWord(wordAsString.Substring(2));
                    return CheckWord(dx).Result;
                }
                else
                {
                    return false;
                }
            });
        }
        public async Task<ICollection<string>> GetPhoneticSuggestions(ProcessedWord word)
        {
            return await Task<ICollection<string>>.Factory.StartNew(() =>
            {
                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word.ToString());

                var retrievedValue1 = _keyValueDatabase.GetValueAsStringByKey(wordHashes.Item1);
                var retrievedValue2 = _keyValueDatabase.GetValueAsStringByKey(wordHashes.Item2);
                if (retrievedValue1 is null && retrievedValue2 is null)
                {
                    return null;
                }
                else
                {
                    List<string> suggestedWords = new();
                    if (retrievedValue1 is not null && retrievedValue1 != String.Empty)
                    {
                        suggestedWords.AddRange(retrievedValue1.Split(','));
                    }
                    if (retrievedValue2 is not null && retrievedValue2 != String.Empty)
                    {
                        suggestedWords.AddRange(retrievedValue2.Split(','));
                    }

                    return suggestedWords;
                }
            });
        }
        public async Task<ICollection<string>> GetRadixTreeSuggestions(ProcessedWord word)
        {
            return await Task<ICollection<string>>.Factory.StartNew(() =>
            {
                var _rt = new ARLeF.Struments.CoretorOrtografic.Core.RadixTree.RadixTree(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH);
                var _checker = new RT_Checker(_rt);

                var wordStr = word.ToString();
                var suggestions = _checker.GetWordsED1(wordStr);

                List<string> result = new List<string>();
                foreach (var suggestion in suggestions)
                {
                    if (suggestion.EndsWith(RT_Checker.NOLC_CAR.ToString()))
                    {
                        var caseWords = GetCaseWords(suggestion.TrimEnd(RT_Checker.NOLC_CAR)).Result;
                        result.AddRange(caseWords);
                    }
                    else
                    {
                        result.Add(suggestion);
                    }
                }

                return result;
            });
        }
        private async Task<IEnumerable<string>> GetCaseWords(string word)
        {
            word = word.ToLower();
            List<string> words = new List<string>();

            var phoneticSuggestions = await GetPhoneticSuggestions(new ProcessedWord(word));

            foreach (var suggestion in phoneticSuggestions)
            {
                if (suggestion.ToLower() == word)
                {
                    words.Add(suggestion);
                }
            }

            return words;
        }

        public void SwapWordWithSuggested(ProcessedWord originalWord, string suggestedWord)
        {
            throw new NotImplementedException();
        }
        public void IgnoreWord(ProcessedWord word)
        {
            throw new NotImplementedException();
        }
        public void AddWord(ProcessedWord word)
        {
            throw new NotImplementedException();
        }

        private ICollection<IProcessedElement> ProcessText(string text)
        {
            List<string> words = Regex.Split(text, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

            foreach (string word in words)
            {
                if (Regex.IsMatch(word, @$"[{FriulianConstants.FRIULIAN_LETTERS}]+"))
                {
                    _processedElements.Add(new ProcessedWord(word));
                }
                else
                {
                    _processedElements.Add(new ProcessedPunctuation(word));
                }
            }
            return _processedElements;
        }
        public string GetProcessedText()
        {
            string result = "";
            foreach (IProcessedElement word in _processedElements)
            {
                result = result + word.ToString();
            }
            return result;
        }

        //public ICollection<IProcessedElement> GetAllProcessedElementsList
        //{
        //    get => _processedElementsList;
        //}
        //public ICollection<ProcessedWord> GetAllProcessedWords
        //{
        //    get => _processedElementsList.OfType<ProcessedWord>().ToList();
        //}
        public ICollection<ProcessedWord> GetAllIncorrectWords()
        {
            return _processedElements.OfType<ProcessedWord>().Where(word => word.Correct == false).ToList();
        }
    }
}
