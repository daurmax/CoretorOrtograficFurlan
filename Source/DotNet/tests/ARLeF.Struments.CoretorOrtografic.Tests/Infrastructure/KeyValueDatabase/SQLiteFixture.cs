using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using LiteDB;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.KeyValueDatabase
{
    public class SQLiteFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = CoretorOrtograficTestDependencyContainer.Configure(true);
        }

        [Test]
        public void ReadValueFromKeyTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            var key = "v8597";
            var expectedResult = "vonde";

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

                if (retrievedData.Any() && retrievedData.Count == 1)
                {
                    Console.WriteLine($"Key is: [{retrievedData.Single().Key}]");
                    Console.WriteLine($"Value is: [{retrievedData.Single().Value}]");

                    Assert.AreEqual(retrievedData.Single().Value, expectedResult);
                }
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
