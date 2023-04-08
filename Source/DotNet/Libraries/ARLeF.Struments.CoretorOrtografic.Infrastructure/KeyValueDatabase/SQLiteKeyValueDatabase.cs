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
    public class SQLiteKeyValueDatabase : IKeyValueDatabase
    {
        public string GetValueAsStringByKey(DictionaryType dictionaryType, string key)
        {
            using (var connection = new SQLiteConnection($"Data Source={GetDictionaryPathByDictionaryType(dictionaryType)}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"SELECT *
                  FROM Words
                  WHERE Key = $key
                ";
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
                    throw new InvalidDataException($"The provided key '{key}' returned more than one result.");
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
        private string GetDictionaryPathByDictionaryType(DictionaryType dictionaryType)
        {
            switch (dictionaryType)
            {
                case DictionaryType.Elisions:
                    return DictionaryFilePaths.SQLITE_ELISIONS_DATABASE_FILE_PATH;
                case DictionaryType.Errors:
                    return DictionaryFilePaths.SQLITE_ERRORS_DATABASE_FILE_PATH;
                case DictionaryType.Frec:
                    return DictionaryFilePaths.SQLITE_FREC_DATABASE_FILE_PATH;
                case DictionaryType.System:
                    return DictionaryFilePaths.SQLITE_WORDS_DATABASE_FILE_PATH;
                default:
                    throw new NotImplementedException("Selected dictionary type is not yet implemented.");
            }
        }
    }
}
