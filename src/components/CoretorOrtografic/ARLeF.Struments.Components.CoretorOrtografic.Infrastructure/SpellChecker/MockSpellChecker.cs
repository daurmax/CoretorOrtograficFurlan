using ARLeF.Struments.Base.Entities;
using ARLeF.Struments.Components.CoretorOrtografic.Core.SpellChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.SpellChecker
{
    public class MockSpellChecker : ISpellChecker
    {
        private ICollection<ProcessedWord> _processedWordList = new List<ProcessedWord>();


        public MockSpellChecker() { }


        public ICollection<ProcessedWord> AllProcessedWordList 
        {
            get => _processedWordList;
        }
        public ICollection<ProcessedWord> AllIncorrectWordList
        {
            get => _processedWordList.Where(word => word.Correct == false).ToList();
        }


        public void ExecuteSpellCheck(string text)
        {
            _processedWordList = GetProcessedWords(text);
            foreach (ProcessedWord word in _processedWordList)
            {
                word.Correct = CheckWordCorrectness(word);
            }
        }

        private ICollection<ProcessedWord> GetProcessedWords(string text)
        {
            foreach (string word in text.Split(' '))
            {
                _processedWordList.Add(new ProcessedWord(word));
            }
            return _processedWordList;
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
