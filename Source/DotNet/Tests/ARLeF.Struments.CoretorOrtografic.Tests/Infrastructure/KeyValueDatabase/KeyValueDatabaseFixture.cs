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
        public void FindInSystemDatabase_WithExistingKey()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "65g8A6597Y7";
                var value = keyValueDatabaseReader.FindInSystemDatabase(key);
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
        public void FindInSystemErrorsDatabase_WithExistingKey()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "adincuatri";
                var value = keyValueDatabaseReader.FindInSystemErrorsDatabase(key);
                var expectedResult = "ad in cuatri";

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.NotNull(value);
                Assert.AreEqual(value, expectedResult);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }

        [Test]
        public void FindInFrequenciesDatabase_WithExistingKey()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "cognossi";
                var value = keyValueDatabaseReader.FindInFrequenciesDatabase(key);
                var expectedResult = 140;

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.NotNull(value);
                Assert.AreEqual(value, expectedResult);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }

        [Test]
        public void FindInSystemDatabase_WithNonExistentKey()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "nonExistentKey";
                var value = keyValueDatabaseReader.FindInSystemDatabase(key);

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.IsNull(value);
            }
        }

        [Test]
        public void FindInSystemErrorsDatabase_WithNonExistentKey()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "ad in cuatri";
                var value = keyValueDatabaseReader.FindInSystemErrorsDatabase(key);

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.IsNull(value);
            }
        }

        [Test]
        public void FindInFrequenciesDatabase_WithNonExistentKey()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                var key = "nonExistentKey";
                var value = keyValueDatabaseReader.FindInFrequenciesDatabase(key);

                Console.WriteLine($"Key is: [{key}]");
                Console.WriteLine($"Value is: [{value}]");

                Assert.IsNull(value);
            }
        }

        [Test]
        public void FindInSystemDatabase_WithNullKey()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                Assert.Throws<ArgumentNullException>(() => keyValueDatabaseReader.FindInSystemDatabase(null));
            }
        }

        [Test]
        public void FindInSystemErrorsDatabase_WithNullKey()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                Assert.Throws<ArgumentNullException>(() => keyValueDatabaseReader.FindInSystemErrorsDatabase(null));
            }
        }

        [Test]
        public void FindInFrequenciesDatabase_WithNullKey()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var keyValueDatabaseReader = scope.Resolve<IKeyValueDatabase>();

                Assert.Throws<ArgumentNullException>(() => keyValueDatabaseReader.FindInFrequenciesDatabase(null));
            }
        }
    }
}
