using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using NUnit.Framework;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Databases
{
    [TestFixture]
    public class FrecSQLiteFixture
    {
        private const string SQLITE_CONNECTION_STRING_FORMAT = "Data Source={0};";

        [Test]
        public void TestFrequencies()
        {
            string[] words = { "cognossi", "cjant", "clap" };
            int[] expectedFrequencies = { 140, 105, 101 };

            string sqliteConnectionString = string.Format(
                SQLITE_CONNECTION_STRING_FORMAT,
                DictionaryFilePaths.SQLITE_FREC_DATABASE_FILE_PATH
            );

            using (var connection = new SqliteConnection(sqliteConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        command.CommandText = "SELECT frequency FROM frequencies WHERE word = @word;";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@word", words[i]);

                        int frequency = 0;
                        using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                frequency = reader.GetInt32(0);
                            }
                        }

                        Assert.AreEqual(expectedFrequencies[i], frequency, $"Frequency for '{words[i]}' should be {expectedFrequencies[i]}");
                    }
                }
            }
        }
    }
}
