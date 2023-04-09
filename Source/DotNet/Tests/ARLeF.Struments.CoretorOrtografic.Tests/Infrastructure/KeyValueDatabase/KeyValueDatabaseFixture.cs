using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Core.Enums;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.KeyValueDatabase
{
    public class KeyValueDatabaseFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = CoretorOrtograficTestDependencyContainer.Configure(true);
        }

        [Test]
        public void ReadValueFromKeyFromSystemDBTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "65g8A6597Y7";
                var value = keyValueDatabaseReader.GetValueAsStringByKey(DictionaryType.SystemDictionary, key);
                var expectedResult = "angossantjure";

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.NotNull(value);
                Assert.AreEqual(value, expectedResult);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }

        [Test]
        public void ReadValueFromKeyFromFrecDBTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "Lessi";
                var value = keyValueDatabaseReader.GetValueAsStringByKey(DictionaryType.Frec, key);
                var expectedResult = @"\15";

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.NotNull(value);
                Assert.AreEqual(value, expectedResult);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
