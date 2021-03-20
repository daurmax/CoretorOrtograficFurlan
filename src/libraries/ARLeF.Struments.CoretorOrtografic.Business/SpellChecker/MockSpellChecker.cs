using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.Struments.CoretorOrtografic.Contracts.SpellChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            foreach (string word in text.Split())
            {
                _processedElementsList.Add(new ProcessedWord(word));
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
    }
}
