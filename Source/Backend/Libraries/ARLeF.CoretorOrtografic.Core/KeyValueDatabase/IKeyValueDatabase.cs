using ARLeF.CoretorOrtografic.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Core.KeyValueDatabase
{
    public interface IKeyValueDatabase
    {
        /// <summary>
        /// Finds a value in the user dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public string FindInUserDatabase(string phoneticHash);

        /// <summary>
        /// Finds a value in the user dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the user dictionary database file is not found.</exception>
        public string FindInUserErrorsDatabase(string word);

        /// <summary>
        /// Finds a value in the system dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the system dictionary database file is not found.</exception>
        public string FindInSystemDatabase(string phoneticHash);

        /// <summary>
        /// Finds the corrected word in the system errors database given a commonly mistaken word.
        /// </summary>
        /// <param name="key">The commonly mistaken word to search in the errors database.</param>
        /// <returns>The corrected word corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the system errors dictionary database file is not found.</exception>
        public string FindInSystemErrorsDatabase(string word);

        /// <summary>
        /// Finds a frequency value in the frequencies database given a word key.
        /// </summary>
        /// <param name="word">The word to search in the frequencies database.</param>
        /// <returns>The frequency value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the frequencies database file is not found.</exception>
        int? FindInFrequenciesDatabase(string word);

        /// <summary>
        /// Checks if the given word exists in the elisions database.
        /// </summary>
        /// <param name="word">The word to search for in the elisions database.</param>
        /// <returns>True if the word is found in the elisions database; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided word is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the elisions database file is not found.</exception>
        bool HasElisions(string word);

        /// <summary>
        /// Adds a word to the user dictionary.
        /// </summary>
        /// <param name="word">The word to be added to the user dictionary.</param>
        /// <returns>A value indicating the result of the operation.</returns>
        public AddWordToUserDatabaseReturnValue AddToUserDatabase(string word);
    }
}
