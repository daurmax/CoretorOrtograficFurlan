using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    public class SQLiteKeyValueDatabase : IKeyValueDatabase
    {
        public string GetValueAsStringByKey(string key)
        {
            using (var connection = new SQLiteConnection($"Data Source={DictionaryFilePaths.SQLITE_WORDS_DATABASE_FILE_PATH}"))
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
                    return retrievedData.Single().Value;
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
    }
}
