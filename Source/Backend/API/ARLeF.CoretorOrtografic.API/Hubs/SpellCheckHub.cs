using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.CoretorOrtografic.Core.SpellChecker;
using Microsoft.AspNetCore.SignalR;

namespace ARLeF.CoretorOrtografic.API.Hubs
{
    public class SpellCheckHub : Hub
    {
        private readonly ISpellChecker _spellChecker;

        public SpellCheckHub(ISpellChecker spellChecker)
        {
            _spellChecker = spellChecker;
        }

        public async Task CheckWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                await Clients.Caller.SendAsync("Error", "Word is required.");
                return;
            }

            _spellChecker.ExecuteSpellCheck(word);
            ProcessedWord processedWord = _spellChecker.ProcessedWords.FirstOrDefault() as ProcessedWord;

            if (processedWord == null)
            {
                await Clients.Caller.SendAsync("Error", "Error processing the word.");
                return;
            }

            await Clients.Caller.SendAsync("ReceiveCheckWordResult", new { Original = processedWord.Original, Current = processedWord.Current, IsCorrect = processedWord.Correct, Case = processedWord.Case });
        }

        public async Task SuggestWords(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                await Clients.Caller.SendAsync("Error", "Word is required.");
                return;
            }

            _spellChecker.ExecuteSpellCheck(word);
            ProcessedWord processedWord = _spellChecker.ProcessedWords.FirstOrDefault() as ProcessedWord;

            if (processedWord == null)
            {
                await Clients.Caller.SendAsync("Error", "Error processing the word.");
                return;
            }

            var suggestions = await _spellChecker.GetWordSuggestions(processedWord);
            await Clients.Caller.SendAsync("ReceiveSuggestWordsResult", new { Original = processedWord.Original, Suggestions = suggestions });
        }
    }
}
