using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;
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
    public class MockSpellChecker : ISpellChecker
    {
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

        public MockSpellChecker() { }


        public async Task ExecuteSpellCheck(string text)
        {
            _processedElements = ProcessText(text);
            foreach (ProcessedWord word in _processedElements)
            {
                word.Correct = await CheckWordCorrectness(word);
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
                Random rand = new Random();
                return rand.NextDouble() > 0.5;
            });
        }
        public async Task<ICollection<string>> GetWordSuggestions(ProcessedWord word)
        {
            return await Task<ICollection<string>>.Factory.StartNew(() =>
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                Random rand = new Random();
                int fakeSuggestions = rand.Next(0, 10);

                List<string> retval = new List<string>();
                for (int i = -1; i < fakeSuggestions; i++)
                {
                    retval.Add(new string(Enumerable.Repeat(chars, rand.Next(4, 15)).Select(s => s[rand.Next(s.Length)]).ToArray()));
                }

                return retval;
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
            List<string> words = Regex.Split(text, "(!?[a-zA-Z-èàòùìç]*)").ToList();
            words = words.Where(x => !String.IsNullOrEmpty(x)).ToList();
            //String.Join(String.Empty, _processedElementsList)

            foreach (string word in words)
            {
                if (Regex.IsMatch(word, "[a-zA-Z-èàòùìç]+"))
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
