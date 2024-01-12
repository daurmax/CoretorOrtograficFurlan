using ARLeF.CoretorOrtografic.Core.Enums;
using ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm;
using ARLeF.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ARLeF.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    /// <summary>
    /// Represents a SQLite key-value database to interact with specific dictionary data.
    /// </summary>
    public class SQLiteKeyValueDatabase : IKeyValueDatabase
    {
        /// <summary>
        /// Finds a value in the user dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        public string FindInUserDatabase(string phoneticHash)
        {
            if (!File.Exists(DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH))
            {
                CreateUserDatabase();
            }

            return FindInDatabase(DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH, DictionaryType.UserDictionary, phoneticHash, false);
        }

        /// <summary>
        /// Finds a value in the user dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the user dictionary database file is not found.</exception>
        public string FindInUserErrorsDatabase(string word)
        {
            return FindInDatabase(DictionaryFilePaths.SQLITE_USER_ERRORS_DATABASE_FILE_PATH, DictionaryType.UserExceptions, word, true);
        }

        /// <summary>
        /// Finds a value in the system dictionary given a phonetic hash key.
        /// </summary>
        /// <param name="phoneticHash">The phonetic hash key calculated using FurlanPhoneticAlgorithm.GetPhoneticHashesByWord() method in the ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm namespace.</param>
        /// <returns>The value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the system dictionary database file is not found.</exception>
        public string FindInSystemDatabase(string phoneticHash)
        {
            return FindInDatabase(DictionaryFilePaths.SQLITE_SYSTEM_DATABASE_FILE_PATH, DictionaryType.SystemDictionary, phoneticHash, false);
        }

        /// <summary>
        /// Finds the corrected word in the system errors database given a commonly mistaken word.
        /// </summary>
        /// <param name="key">The commonly mistaken word to search in the errors database.</param>
        /// <returns>The corrected word corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the system errors dictionary database file is not found.</exception>
        public string FindInSystemErrorsDatabase(string word)
        {
            return FindInDatabase(DictionaryFilePaths.SQLITE_ERRORS_DATABASE_FILE_PATH, DictionaryType.SystemErrors, word, true);
        }

        /// <summary>
        /// Finds a frequency value in the frequencies database given a word key.
        /// </summary>
        /// <param name="word">The word to search in the frequencies database.</param>
        /// <returns>The frequency value corresponding to the given key, or null if not found.</returns>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the frequencies database file is not found.</exception>
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
                        if (reader.GetValue(1).GetType() != typeof(System.DBNull))
                        {
                            retrievedData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }
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

        /// <summary>
        /// Checks if the given word exists in the elisions database.
        /// </summary>
        /// <param name="word">The word to search for in the elisions database.</param>
        /// <returns>True if the word is found in the elisions database; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided word is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the elisions database file is not found.</exception>
        public bool HasElisions(string word)
        {
            if (string.IsNullOrEmpty(word)) throw new ArgumentNullException();

            if (!File.Exists(DictionaryFilePaths.SQLITE_ELISIONS_DATABASE_FILE_PATH))
            {
                throw new FileNotFoundException($"{DictionaryType.Elisions} database was not found at '{Path.GetFullPath(DictionaryFilePaths.SQLITE_ELISIONS_DATABASE_FILE_PATH)}'");
            }

            using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_ELISIONS_DATABASE_FILE_PATH}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"SELECT *
          FROM Data
          WHERE Word = $word
        ";
                command.Parameters.AddWithValue("$word", word);

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// Adds a word to the user dictionary.
        /// </summary>
        /// <param name="word">The word to be added to the user dictionary.</param>
        /// <returns>A value indicating the result of the operation.</returns>
        public AddWordToUserDatabaseReturnValue AddToUserDatabase(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return AddWordToUserDatabaseReturnValue.UserDatabaseNotExists;
            }

            using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH}"))
            {
                connection.Open();

                (string, string) wordHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);
                string codeA = wordHashes.Item1;
                string codeB = wordHashes.Item2;

                foreach (string code in codeA.Equals(codeB) ? new[] { codeA } : new[] { codeA, codeB })
                {
                    string foundWord = FindInUserDatabase(code);

                    if (foundWord == null)
                    {
                        var insertCommand = connection.CreateCommand();
                        insertCommand.CommandText =
                        @"INSERT INTO Data (Key, Value)
                          VALUES ($key, $value)
                        ";
                        insertCommand.Parameters.AddWithValue("$key", code);
                        insertCommand.Parameters.AddWithValue("$value", word);
                        insertCommand.ExecuteNonQuery();
                    }
                    else if (!foundWord.Equals(word))
                    {
                        string newWordList = $"{foundWord},{word}";

                        var updateCommand = connection.CreateCommand();
                        updateCommand.CommandText =
                        @"UPDATE Data
                          SET Value = $value
                          WHERE Key = $key
                        ";
                        updateCommand.Parameters.AddWithValue("$key", code);
                        updateCommand.Parameters.AddWithValue("$value", newWordList);
                        updateCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        return AddWordToUserDatabaseReturnValue.AlreadyPresent;
                    }
                }
            }

            return AddWordToUserDatabaseReturnValue.Success;
        }

        /// <summary>
        /// Finds a value in the specified database given a key.
        /// </summary>
        /// <param name="databaseFilePath">The file path of the SQLite database to search in.</param>
        /// <param name="key">The key to search for in the database.</param>
        /// <param name="searchForErrors">Indicates whether the search is being performed in the errors database.</param>
        /// <returns>The value corresponding to the given key, or null if not found or the database does not exist.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided key is null or empty.</exception>
        /// <exception cref="InvalidDataException">Thrown when the provided key returns more than one result.</exception>
        private string FindInDatabase(string databaseFilePath, DictionaryType dictionaryType, string key, bool searchForErrors)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException();

            if (!File.Exists(databaseFilePath))
            {
                throw new FileNotFoundException($"{dictionaryType} database was not found at '{Path.GetFullPath(DictionaryFilePaths.SQLITE_ELISIONS_DATABASE_FILE_PATH)}'");
            }

            using (var connection = new SQLiteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                // Choose the table name based on the DictionaryType
                string tableName = dictionaryType == DictionaryType.SystemDictionary ? "Words" : "Data";
                command.CommandText = $"SELECT * FROM {tableName} WHERE Key = $key";
                command.Parameters.AddWithValue("$key", key);

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
                    string exceptionMessage = searchForErrors
                        ? $"The provided key '{key}' returned more than one result in the errors database."
                        : $"The provided key '{key}' returned more than one result.";

                    throw new InvalidDataException(exceptionMessage);
                }
            }
        }

        /// <summary>
        /// Creates the user database if it does not already exist.
        /// </summary>
        /// <returns>A value indicating whether the database was created or already exists.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the operation to create the database fails.</exception>
        public void CreateUserDatabase()
        {
            if (!File.Exists(DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH));
                SQLiteConnection.CreateFile(DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH);

                using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_USER_DATABASE_FILE_PATH}"))
                {
                    connection.Open();

                    var createTableCommand = connection.CreateCommand();
                    createTableCommand.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Data (
              Key TEXT NOT NULL,
              Value TEXT NOT NULL,
              PRIMARY KEY (Key, Value)
            )";
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Replaces the Unicode codes in a word with their corresponding special characters.
        /// </summary>
        /// <param name="word">The word with Unicode codes to replace.</param>
        /// <returns>The word with special characters.</returns>
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
