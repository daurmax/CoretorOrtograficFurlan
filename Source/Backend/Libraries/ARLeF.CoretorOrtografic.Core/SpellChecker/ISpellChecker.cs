using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Core.SpellChecker
{
    public interface ISpellChecker
    {
        /// <summary>
        /// Unmutable collection of all processed elements.
        /// </summary>
        IReadOnlyCollection<IProcessedElement> ProcessedElements { get; }

        // <summary>
        /// Unmutable collection containing only processed words.
        /// </summary>
        IReadOnlyCollection<IProcessedElement> ProcessedWords { get; }


        /// <summary>
        /// Executes the spell check on a given <see cref="string"/>
        /// </summary>
        /// <param name="text"><see cref="string"/> to spell check</param>
        void ExecuteSpellCheck(string text);

        /// <summary>
        /// Cleans the spell checker.
        /// </summary>
        void CleanSpellChecker();

        /// <summary>
        /// Checks if the given <see cref="ProcessedWord"/> is correct
        /// </summary>
        /// <param name="word"><see cref="ProcessedWord"/> to be checked</param>
        /// <returns>Boolean indicating if the <see cref="ProcessedWord"/> is correct</returns>
        Task<bool> CheckWord(ProcessedWord word);

        Task<ICollection<string>> GetWordSuggestions(ProcessedWord word);

        /// <summary>
        /// Swaps the incorrect <see cref="ProcessedWord"/> with the suggested one
        /// </summary>
        /// <param name="originalWord"><see cref="ProcessedWord"/> to be replaced</param>
        /// <param name="suggestedWord">new word to replace the original one with</param>
        void SwapWordWithSuggested(ProcessedWord originalWord, string suggestedWord);

        /// <summary>
        /// Ignores the given <see cref="ProcessedWord"/>, skipping it
        /// </summary>
        /// <param name="word"><see cref="ProcessedWord"/> to be skipped</param>
        void IgnoreWord(ProcessedWord word);

        /// <summary>
        /// Adds the given <see cref="ProcessedWord"/> to the dictionary
        /// </summary>
        /// <param name="word"><see cref="ProcessedWord"/> to be added to the dictionary</param>
        void AddWord(ProcessedWord word);

        /// <summary>
        /// Returns the corrected text concatenating all the corrected <see cref="ProcessedWord"/>s and <see cref="ProcessedPunctuation"/>s
        /// </summary>
        /// <returns><see cref="string"/> containing the corrected text</returns>
        string GetProcessedText();

        /// <summary>
        /// Retrieves a list of all incorrect <see cref="ProcessedWord"/>s
        /// </summary>
        /// <returns><see cref="ICollection{ProcessedWord}"/> of all incorrect words</returns>
        ICollection<ProcessedWord> GetAllIncorrectWords();
    }
}
