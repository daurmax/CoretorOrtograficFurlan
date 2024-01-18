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
            await Clients.Caller.SendAsync("ReceiveSuggestWordsResult", new { Word = processedWord.Original, Suggestions = suggestions, IsCorrect = processedWord.Correct });
        }

        public async Task CheckText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                await Clients.Caller.SendAsync("Error", "Text is required.");
                return;
            }

            _spellChecker.ExecuteSpellCheck(text);

            // Process results and construct response
            var processedTextResults = _spellChecker.ProcessedElements
                                                    .OfType<ProcessedWord>()
                                                    .Select(pw => new
                                                    {
                                                        pw.Original,
                                                        pw.Current,
                                                        pw.Correct,
                                                        pw.Case,
                                                        pw.Suggestions
                                                    }).ToList();

            if (processedTextResults.Count == 0)
            {
                await Clients.Caller.SendAsync("Error", "Error processing the text.");
                return;
            }

            await Clients.Caller.SendAsync("ReceiveCheckTextResult", processedTextResults);
        }
    }
}
