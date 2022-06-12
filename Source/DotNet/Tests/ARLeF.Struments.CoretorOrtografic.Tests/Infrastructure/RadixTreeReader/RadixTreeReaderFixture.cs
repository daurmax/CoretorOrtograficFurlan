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

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTree
{
    public class RadixTreeReaderFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = CoretorOrtograficTestDependencyContainer.Configure(true);
        }

        [Test]
        public void ReadWordsRadixTreeTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = Container.BeginLifetimeScope())
            {
                var radixTreeReader = scope.Resolve<IRadixTreeReader>();

                RadixTreeNode rootNode = radixTreeReader.RootNode;

                Console.WriteLine($"Edge position: [{rootNode.EdgePosition}]");
                Console.WriteLine($"Number of edges: [{rootNode.EdgesNumber}]");
                Console.WriteLine($"Next position value: [{rootNode.NextPosition}]");
                Console.WriteLine($"Next number value: [{rootNode.NextNumber}]");

                Assert.NotNull(rootNode);
                Assert.AreEqual(0, rootNode.EdgePosition);
                Assert.AreEqual(38, rootNode.EdgesNumber);
                Assert.AreEqual(0, rootNode.NextNumber);
                Assert.AreEqual(1, rootNode.NextPosition);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
