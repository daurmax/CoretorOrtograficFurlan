using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Core.SpellChecker
{
    public interface ISpellChecker
    {
        /// <summary>
        /// Analyzes the text and returns an ICollection of <see cref="ProcessedWord"/>s
        /// </summary>
        /// <param name="text">Raw input text as <see cref="string"/></param>
        /// <returns>ICollection of <see cref="ProcessedWord"/>s to be checked</returns>
        //ICollection<ProcessedWord> GetProcessedWords(string text);

        /// <summary>
        /// Executes the spell check on a given <see cref="string"/>
        /// </summary>
        /// <param name="text"><see cref="string"/> to spell check</param>
        void ExecuteSpellCheck(string text);

        /// <summary>
        /// Checks if the given <see cref="ProcessedWord"/> is correct
        /// </summary>
        /// <param name="word"><see cref="ProcessedWord"/> to be checked</param>
        /// <returns>Boolean indicating if the <see cref="ProcessedWord"/> is correct</returns>
        bool CheckWordCorrectness(ProcessedWord word);

        /// <summary>
        /// Retrieves a list of suggested words to be swapped with the given <see cref="ProcessedWord"/>
        /// </summary>
        /// <param name="word"><see cref="ProcessedWord"/> to be swapped with suggested ones</param>
        /// <returns><see cref="ICollection{ProcessedWord}"/> of suggested words</returns>
        ICollection<string> GetSuggestions(ProcessedWord word);

        /// <summary>
        /// Swaps the incorrect <see cref="ProcessedWord"/> with the suggested one
        /// </summary>
        /// <param name="originalWord"><see cref="ProcessedWord"/> to be replaced</param>
        /// <param name="suggestedWord">new word to replace the original one with</param>
        void SwapWord(ProcessedWord originalWord, string suggestedWord);

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
    }
}
