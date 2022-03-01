using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Entities.RadixTree;
using NUnit.Framework;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTree
{
    public class RadixTreeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadWordsRadixTreeTest()
        {
            string RADIX_FILE_PATH = "Dictionaries/WordsRadixTree/words.rt";

            var timer = new Stopwatch();
            timer.Start();

            var rootNode = new RadixTreeNode(RADIX_FILE_PATH, 0);

            Console.WriteLine($"Edge position: [{rootNode.EdgePosition}]");
            Console.WriteLine($"Number of edges: [{rootNode.EdgesNumber}]");
            Console.WriteLine($"Next position value: [{rootNode.NextPosition}]");
            Console.WriteLine($"Next number value: [{rootNode.NextNumber}]");

            Assert.NotNull(rootNode);
            Assert.AreEqual(0, rootNode.EdgePosition);
            Assert.AreEqual(38, rootNode.EdgesNumber);
            Assert.AreEqual(0, rootNode.NextNumber);
            Assert.AreEqual(1, rootNode.NextPosition);

            timer.Stop();
        }
    }
}
