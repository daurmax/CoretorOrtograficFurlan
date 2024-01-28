using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.CoretorOrtografic.Core.Constants;
using ARLeF.CoretorOrtografic.Core.Enums;
using ARLeF.CoretorOrtografic.Core.Extensions;
using ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm;
using ARLeF.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.CoretorOrtografic.Core.SpellChecker;
using ARLeF.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Infrastructure.SpellChecker
{
    public class FurlanSpellChecker : ISpellChecker
    {
        private const int MAX_SUGGESTIONS = 10;

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

        #region Public methods

        public void ExecuteSpellCheck(string text)
        {
            _processedElements = ProcessText(text);
            foreach (ProcessedWord word in ProcessedWords)
            {
                try
                {
                    // Skip single-character words
                    if (word.Original.Length > 3)
                    {
                        word.Correct = CheckWord(word).Result;
                        if (!word.Correct)
                        {
                            word.Suggestions = GetWordSuggestions(word).Result.ToList();
                        }
                    }
                    else
                    {
                        word.Correct = true;
                    }
                }
                catch (Exception)
                {
                    word.Correct = false;

                    word.Suggestions = new List<string>();
                }
            }
        }
        public void CleanSpellChecker()
        {
            _processedElements = new List<IProcessedElement>();
        }

        public async Task<bool> CheckWord(ProcessedWord word)
        {
            return await Task.Run(async () =>
            {
                string wordAsString = word.Original;
                string lcWord = wordAsString.ToLower();

                if (Regex.IsMatch(wordAsString, @"\d|^[^" + FriulianConstants.WORD_LETTERS + "]+$"))
                {
                    return true;
                }

                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(lcWord);

                ICollection<string> suggestedWords = await GetSystemDictionaryPhoneticSuggestions(word);

                bool wordFoundInSuggestions = suggestedWords is not null && suggestedWords.Any(s => s.ToLower() == lcWord);

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
                    var userSearchResult = await GetUserDictionaryPhoneticSuggestions(word);
                    bool userWordFound = userSearchResult is not null && userSearchResult.Any();
                    return userWordFound;
                }
            });
        }
        public async Task<ICollection<string>> GetWordSuggestions(ProcessedWord word)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            var (weights, suggestions) = await SuggestRaw(word);
            var sortedSuggestions = new List<string>();

            foreach (var f in weights.Keys.OrderByDescending(k => k))
            {
                foreach (var d in weights[f].Keys.OrderBy(k => k))
                {
                    foreach (var index in weights[f][d])
                    {
                        sortedSuggestions.Add(suggestions[index]);
                    }
                }
            }

            return sortedSuggestions.Take(MAX_SUGGESTIONS).ToList();
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

        public string GetProcessedText()
        {
            string result = "";
            foreach (IProcessedElement word in _processedElements)
            {
                result = result + word.ToString();
            }
            return result;
        }

        public ICollection<ProcessedWord> GetAllIncorrectWords()
        {
            return _processedElements.OfType<ProcessedWord>().Where(word => word.Correct == false).ToList();
        }

        #endregion Public methods

        #region Private methods

        private string FixCase(WordType wordCase, string word)
        {
            if (wordCase == WordType.Lowercase)
            {
                return word;
            }

            string lcWord = word.ToLower();
            string ucWord = lcWord.ToUpper();
            string ucfWord = word.ToFirstCharacterUpper();

            if (word == lcWord || word == ucfWord)
            {
                return wordCase == WordType.FirstLetterUppercase ? ucfWord : ucWord;
            }

            return word;
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

        private async Task<(Dictionary<int, Dictionary<int, List<int>>>, List<string>)> SuggestRaw(ProcessedWord word)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            var list = await BuildSuggestions(word);

            var foundWords = new List<string>();
            foreach (var p in list.Keys)
            {
                foundWords.Add(p);
            }

            foundWords = FurlanPhoneticAlgorithm.SortFriulian(foundWords);
            var wordsHamming = new Dictionary<int, Dictionary<int, List<int>>>();

            for (int wordIndex = 0; wordIndex < foundWords.Count; wordIndex++)
            {
                var y = foundWords[wordIndex];
                var vals = list[y];

                if (!wordsHamming.ContainsKey(vals[0]))
                {
                    wordsHamming[vals[0]] = new Dictionary<int, List<int>>();
                }

                if (!wordsHamming[vals[0]].ContainsKey(vals[1]))
                {
                    wordsHamming[vals[0]][vals[1]] = new List<int>();
                }

                wordsHamming[vals[0]][vals[1]].Add(wordIndex);
            }

            return (wordsHamming, foundWords);
        }
        private async Task<Dictionary<string, List<int>>> BuildSuggestions(ProcessedWord word)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            var list = new Dictionary<string, List<int>>(await BasicSuggestions(word));
            var lcWord = word.Original.ToLower();
            var caseType = word.Case;

            if (lcWord.StartsWith("d'") || lcWord.StartsWith("un'"))
            {
                int pos = lcWord.StartsWith("d'") ? 2 : 3;
                string sx = pos == 2 ? "di" : "une";
                var dxWord = new ProcessedWord(word.Original.Substring(pos));

                var dxList = await BasicSuggestions(dxWord);
                foreach (var (p, vals) in dxList)
                {
                    string fixedP = FixCase(caseType, sx + " " + p);
                    vals[1]++;
                    list[fixedP] = vals;
                }
            }
            else if (lcWord.StartsWith("l'"))
            {
                var dxWord = new ProcessedWord(word.Original.Substring(2));

                var sxAp = FixCase(caseType, "l'");
                var sxNoAp = FixCase(caseType, "la") + " ";

                var dxList = await BasicSuggestions(dxWord, true);
                foreach (var (p, vals) in dxList)
                {
                    string sx = _keyValueDatabase.HasElisions(p) ? sxAp : sxNoAp;
                    string fixedP = FixCase(caseType, sx + p);
                    vals[1]++;
                    list[fixedP] = vals;
                }
            }

            if (word.Original.Contains("-"))
            {
                string[] parts = word.Original.Split('-');
                var sxWord = new ProcessedWord(parts[0]);
                var dxWord = new ProcessedWord(parts[1]);

                var sxList = await BasicSuggestions(sxWord);
                var dxList = await BasicSuggestions(dxWord);

                foreach (var (sxP, sxVals) in sxList)
                {
                    foreach (var (dxP, dxVals) in dxList)
                    {
                        string combinedP = $"{sxP} {dxP}";
                        list[combinedP] = new List<int>
                {
                    sxVals[0] + dxVals[0],
                    sxVals[1] + dxVals[1]
                };
                    }
                }
            }

            return list;
        }
        private async Task<Dictionary<string, List<int>>> BasicSuggestions(ProcessedWord word, bool dizWord = false)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            var wordText = word.ToString();
            var lcWord = word.Original.ToLower();
            var caseType = word.Case;

            var list = new Dictionary<string, List<int>>();
            var sugg = new Dictionary<string, int>();

            var systemDictionarySuggestions = await GetSystemDictionaryPhoneticSuggestions(word);
            if (systemDictionarySuggestions != null)
            {
                foreach (var suggestedWord in await GetSystemDictionaryPhoneticSuggestions(word))
                {
                    sugg[suggestedWord] = 5;
                }
            }

            //if (Settings.HasUserDictionary)
            if (false)
            {
                var userDictionarySuggestions = await GetUserDictionaryPhoneticSuggestions(word);
                if (userDictionarySuggestions != null)
                {
                    foreach (var suggestedWord in await GetUserDictionaryPhoneticSuggestions(word))
                    {
                        sugg[suggestedWord] = 4;
                    }
                }
            }

            var radixTreeSuggestions = await GetRadixTreeSuggestions(word);
            if (radixTreeSuggestions != null)
            {
                foreach (var suggestedWord in await GetRadixTreeSuggestions(word))
                {
                    sugg[suggestedWord] = 3;
                }
            }

            var cor = await FindInExceptions(word: word, isSystemDictionary: true);
            if (cor != null)
            {
                sugg[cor] = 2;
            }

            //if (Settings.HasExceptionsDictionary)
            if (false)
            {
                //var cor = await FindInExceptions(word: word, isSystemDictionary: false);
                //if (cor != null)
                //{
                //    sugg[cor] = 1;
                //}
            }

            foreach (var (p, type) in sugg)
            {
                var fixedP = FixCase(caseType, p);
                if (!list.ContainsKey(fixedP))
                {
                    var vals = new List<int>();
                    var lcP = p.ToLower();
                    if (lcWord == lcP)
                    {
                        vals.Add((int)SuggestionOriginPriorityValue.Same);
                        vals.Add(1);
                    }
                    else if (type == 1)
                    {
                        vals.Add((int)SuggestionOriginPriorityValue.UserException);
                        vals.Add(0);
                    }
                    else if (type == 2)
                    {
                        vals.Add((int)SuggestionOriginPriorityValue.SystemErrors);
                        vals.Add(0);
                    }
                    else if (type == 3)
                    {
                        vals.Add(_keyValueDatabase.FindInFrequenciesDatabase(p) ?? 0);
                        vals.Add(1);
                    }
                    else if (type == 4)
                    {
                        vals.Add((int)SuggestionOriginPriorityValue.UserDictionary);
                        vals.Add(FurlanPhoneticAlgorithm.Levenshtein(lcWord, p));
                    }
                    else
                    {
                        vals.Add(_keyValueDatabase.FindInFrequenciesDatabase(p) ?? 0);
                        vals.Add(FurlanPhoneticAlgorithm.Levenshtein(lcWord, p));
                    }
                    if (dizWord)
                    {
                        vals.Add(type);
                    }
                    list[fixedP] = vals;
                }
            }

            return list;
        }

        private async Task<ICollection<string>> GetSystemDictionaryPhoneticSuggestions(ProcessedWord word)
        {
            return await Task<ICollection<string>>.Factory.StartNew(() =>
            {
                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word.ToString());

                var retrievedValue1 = _keyValueDatabase.FindInSystemDatabase(wordHashes.Item1);
                var retrievedValue2 = _keyValueDatabase.FindInSystemDatabase(wordHashes.Item2);
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
        private async Task<ICollection<string>> GetUserDictionaryPhoneticSuggestions(ProcessedWord word)
        {
            return await Task<ICollection<string>>.Factory.StartNew(() =>
            {
                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word.ToString());

                var retrievedValue1 = _keyValueDatabase.FindInUserDatabase(wordHashes.Item1);
                var retrievedValue2 = _keyValueDatabase.FindInUserDatabase(wordHashes.Item2);
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
        private async Task<string> FindInExceptions(ProcessedWord word, bool isSystemDictionary)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string wordStr = word.ToString();
                string correction = isSystemDictionary ? _keyValueDatabase.FindInSystemErrorsDatabase(wordStr) : _keyValueDatabase.FindInUserErrorsDatabase(wordStr);

                if (correction != null)
                {
                    return correction;
                }
                else
                {
                    WordType caseType = word.Case;
                    if (caseType == WordType.Uppercase)
                    {
                        return null;
                    }
                    else if (caseType == WordType.Lowercase)
                    {
                        correction = isSystemDictionary ? _keyValueDatabase.FindInSystemErrorsDatabase(word.ToString().ToLower()) : _keyValueDatabase.FindInUserDatabase(word.ToString().ToLower());
                        return correction;
                    }
                    else
                    {
                        correction = isSystemDictionary ? _keyValueDatabase.FindInSystemErrorsDatabase(word.ToString().ToLower()) : _keyValueDatabase.FindInUserDatabase(word.ToString().ToLower());

                        if (correction != null)
                        {
                            return correction;
                        }
                        else
                        {
                            correction = isSystemDictionary ? _keyValueDatabase.FindInSystemErrorsDatabase(word.ToString().ToFirstCharacterUpper()) : _keyValueDatabase.FindInUserDatabase(word.ToString().ToFirstCharacterUpper());
                            return correction;
                        }
                    }
                }
            });
        }
        private async Task<ICollection<string>> GetRadixTreeSuggestions(ProcessedWord word)
        {
            return await Task<ICollection<string>>.Factory.StartNew(() =>
            {
                var _rt = new ARLeF.CoretorOrtografic.Core.RadixTree.RadixTree(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH);
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

            var phoneticSuggestions = await GetSystemDictionaryPhoneticSuggestions(new ProcessedWord(word));

            if (phoneticSuggestions != null)
            {
                foreach (var suggestion in phoneticSuggestions)
                {
                    if (suggestion.ToLower() == word)
                    {
                        words.Add(suggestion);
                    }
                }
            }

            return words;
        }

        #endregion Private methods
    }
}
