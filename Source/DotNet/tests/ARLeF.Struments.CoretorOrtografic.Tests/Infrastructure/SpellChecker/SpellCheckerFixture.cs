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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ARLeF.Struments.CoretorOrtografic.Core.Enums;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.SpellChecker
{
    public class SpellCheckerFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = CoretorOrtograficTestDependencyContainer.Configure(true);
        }

        [Test]
        public void SplitWordsTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key1 = "g6v7";
                var value1 = keyValueDatabaseReader.GetValueAsStringByKey(DictionaryType.SystemDictionary, key1);
                var key2 = "E6v7";
                var value2 = keyValueDatabaseReader.GetValueAsStringByKey(DictionaryType.SystemDictionary, key2);

                List<string> result = new();
                if (value1 != null)
                {
                    result.AddRange(value1?.Split(','));
                }
                if (value2 != null)
                {
                    result.AddRange(value2?.Split(','));
                }
                result = result.Distinct().ToList();

                Console.WriteLine($"Key1 is: [{key1}]");
                Console.WriteLine($"Key2 is: [{key2}]");
                Console.WriteLine($"Value1 is: [{value1}]");
                Console.WriteLine($"Value2 is: [{value2}]");

                Assert.NotNull(value1);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
