using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.Struments.CoretorOrtografic.Contracts.SpellChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Business.SpellChecker
{
    public class MockSpellChecker : ISpellChecker
    {
        private ICollection<IProcessedElement> _processedElementsList = new List<IProcessedElement>();


        public MockSpellChecker() { }


        public ICollection<IProcessedElement> AllProcessedElementsList 
        {
            get => _processedElementsList;
        }
        public ICollection<ProcessedWord> AllProcessedWords
        {
            get => _processedElementsList.OfType<ProcessedWord>().ToList();
        }
        public ICollection<ProcessedWord> AllIncorrectWordList
        {
            get => _processedElementsList.OfType<ProcessedWord>().Where(word => word.Correct == false).ToList();
        }


        public void ExecuteSpellCheck(string text)
        {
            _processedElementsList = GetProcessedElements(text);
            foreach (ProcessedWord word in _processedElementsList)
            {
                word.Correct = CheckWordCorrectness(word);
            }
        }

        private ICollection<IProcessedElement> GetProcessedElements(string text)
        {
            List<string> words = Regex.Split(text, "(!?[a-zA-Z-èàòùìç]*)").ToList();
            words = words.Where(x => !String.IsNullOrEmpty(x)).ToList();
            //String.Join(String.Empty, _processedElementsList)

            foreach (string word in words)
            {
                if (Regex.IsMatch(word, "[a-zA-Z-èàòùìç]+"))
                {
                    _processedElementsList.Add(new ProcessedWord(word));
                } else
                {
                    _processedElementsList.Add(new ProcessedPunctuation(word));
                }
            }
            return _processedElementsList;
        }
        public bool CheckWordCorrectness(ProcessedWord word)
        {
            Random rand = new Random();
            return rand.NextDouble() > 0.5;
        }

        public ICollection<string> GetSuggestions(ProcessedWord word)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random rand = new Random();
            int fakeSuggestions = rand.Next(0, 10);

            List<string> retval = new List<string>();
            for (int i = -1; i < fakeSuggestions; i++)
            {
                retval.Add(new string(Enumerable.Repeat(chars, rand.Next(4,15)).Select(s => s[rand.Next(s.Length)]).ToArray()));
            }

            return retval;
        }

        public void SwapWord(ProcessedWord originalWord, string suggestedWord)
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

        public string GetCorrectedText()
        {
            string result = "";
            foreach(IProcessedElement word in _processedElementsList)
            {
                result = result + word.ToString();
            }
            return result;
        }
    }
}
