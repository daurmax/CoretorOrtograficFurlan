using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.Struments.CoretorOrtografic.Core.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
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
                word.Correct = CheckWordCorrectness(word).Result;
            }
        }
        public void CleanSpellChecker()
        {
            _processedElements = new List<IProcessedElement>();
        }

        public async Task<bool> CheckWordCorrectness(ProcessedWord word)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word.ToString());

                var retrievedValue1 = _keyValueDatabase.GetValueAsStringByKey(wordHashes.Item1);
                var retrievedValue2 = _keyValueDatabase.GetValueAsStringByKey(wordHashes.Item2);
                if (retrievedValue1 is null && retrievedValue2 is null)
                {
                    return false;
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

                    return suggestedWords.Contains(word.ToString());
                }
            });
        }
        public async Task<ICollection<string>> GetWordSuggestions(ProcessedWord word)
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
            List<string> words = Regex.Split(text, @$"(!?[{FriulianConstants.FRIULIAN_LETTERS}]*)").Where(x => !String.IsNullOrEmpty(x)).ToList();
            //String.Join(String.Empty, _processedElementsList)

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
