using ARLeF.Struments.CoretorOrtografic.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase
{
    public interface IKeyValueDatabase
    {
        /// <summary>
        /// Finds a value in the system dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public string FindInSystemDatabase(string phoneticHash);

        /// <summary>
        /// Finds the corrected word in the system errors database given a commonly mistaken word.
        /// </summary>
        /// <param name="key">The commonly mistaken word to search in the errors database.</param>
        /// <returns>The corrected word corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public string FindInSystemErrorsDatabase(string word);

        /// <summary>
        /// Finds a frequency value in the frequencies database given a word key.
        /// </summary>
        /// <param name="word">The word to search in the frequencies database.</param>
        /// <returns>The frequency value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        int? FindInFrequenciesDatabase(string word);
    }
}
