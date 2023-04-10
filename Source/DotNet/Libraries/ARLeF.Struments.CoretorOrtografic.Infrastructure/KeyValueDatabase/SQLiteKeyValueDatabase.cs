using ARLeF.Struments.CoretorOrtografic.Core.Enums;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    /// <summary>
    /// Represents a SQLite key-value database to interact with specific dictionary data.
    /// </summary>
    public class SQLiteKeyValueDatabase : IKeyValueDatabase
    {
        /// <summary>
        /// Finds a value in the system dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public string FindInSystemDatabase(string phoneticHash)
        {
            if (string.IsNullOrEmpty(phoneticHash)) throw new ArgumentNullException();

            using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_WORDS_DATABASE_FILE_PATH}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"SELECT *
                  FROM Data
                  WHERE Key = $key
                ";
                command.Parameters.AddWithValue("$key", phoneticHash);

                List<KeyValuePair<string, string>> retrievedData = new();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedData.Add(new KeyValuePair<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }

                if (retrievedData.Count == 1)
                {
                    return ReplaceWordUnicodeCodesWithSpecialCharacters(retrievedData.Single().Value);
                }
                else if (!retrievedData.Any())
                {
                    return null;
                }
                else
                {
                    throw new InvalidDataException($"The provided key '{phoneticHash}' returned more than one result.");
                }
            }
        }

        /// <summary>
        /// Finds the corrected word in the system errors database given a commonly mistaken word.
        /// </summary>
        /// <param name="key">The commonly mistaken word to search in the errors database.</param>
        /// <returns>The corrected word corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public string FindInSystemErrorsDatabase(string word)
        {
            if (string.IsNullOrEmpty(word)) throw new ArgumentNullException();

            using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_ERRORS_DATABASE_FILE_PATH}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"SELECT *
                  FROM Data
                  WHERE Key = $key
                ";
                command.Parameters.AddWithValue("$key", word);

                List<KeyValuePair<string, string>> retrievedData = new();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedData.Add(new KeyValuePair<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }

                if (retrievedData.Count == 1)
                {
                    return ReplaceWordUnicodeCodesWithSpecialCharacters(retrievedData.Single().Value);
                }
                else if (!retrievedData.Any())
                {
                    return null;
                }
                else
                {
                    throw new InvalidDataException($"The provided key '{word}' returned more than one result.");
                }
            }
        }

        /// <summary>
        /// Finds a frequency value in the frequencies database given a word key.
        /// </summary>
        /// <param name="word">The word to search in the frequencies database.</param>
        /// <returns>The frequency value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public int? FindInFrequenciesDatabase(string word)
        {
            if (string.IsNullOrEmpty(word)) throw new ArgumentNullException();

            using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_FREC_DATABASE_FILE_PATH}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"SELECT *
                  FROM Data
                  WHERE Key = $key
                ";
                command.Parameters.AddWithValue("$key", word);

                List<KeyValuePair<string, int>> retrievedData = new();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                    }
                }

                if (retrievedData.Count == 1)
                {
                    return retrievedData.Single().Value;
                }
                else if (!retrievedData.Any())
                {
                    return null;
                }
                else
                {
                    throw new InvalidDataException($"The provided key '{word}' returned more than one result.");
                }
            }
        }

        private string ReplaceWordUnicodeCodesWithSpecialCharacters(string word)
        {
            var retval = word;

            retval = Regex.Replace(retval, @"\\e7", "ç");
            retval = Regex.Replace(retval, @"\\e2", "â");
            retval = Regex.Replace(retval, @"\\ea", "ê");
            retval = Regex.Replace(retval, @"\\ee", "î");
            retval = Regex.Replace(retval, @"\\f4", "ô");
            retval = Regex.Replace(retval, @"\\fb", "û");
            retval = Regex.Replace(retval, @"\\e0", "à");
            retval = Regex.Replace(retval, @"\\e8", "è");
            retval = Regex.Replace(retval, @"\\ec", "ì");
            retval = Regex.Replace(retval, @"\\f2", "ò");
            retval = Regex.Replace(retval, @"\\f9", "ù");

            return retval;
        }
    }
}
