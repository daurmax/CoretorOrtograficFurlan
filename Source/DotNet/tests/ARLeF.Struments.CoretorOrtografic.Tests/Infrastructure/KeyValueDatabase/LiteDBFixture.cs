using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using LiteDB;
using System.Linq;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.KeyValueDatabase
{
    public class LiteDBFixture
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

            using (var db = new LiteDatabase(DictionaryFilePaths.LITEDB_WORDS_DATABASE_FILE_PATH))
            {
                var wordsCollection = db.GetCollection<BsonDocument>("words");

                var key = "v8597";
                var value = wordsCollection.FindOne(Query.EQ("_id", key));
                var expectedResult = "vonde";

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value["value"].AsString}]");

                Assert.NotNull(value["value"]);
                Assert.AreEqual(value["value"].AsString, expectedResult);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
