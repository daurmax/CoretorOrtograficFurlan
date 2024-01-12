using ARLeF.CoretorOrtografic.Core.RadixTree;
using ARLeF.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.CoretorOrtografic.Infrastructure.RadixTreeDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Tests.Core.RadixTree
{
    public class RadixTreeFixture
    {
        private ARLeF.CoretorOrtografic.Core.RadixTree.RadixTree _radixTree;

        [SetUp]
        public void Setup()
        {
            _radixTree = new ARLeF.CoretorOrtografic.Core.RadixTree.RadixTree(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH);
        }

        [Test]
        public void GetRoot_Returns_RadixTreeNode_With_Correct_EdgeNumber()
        {
            // Arrange
            int expectedEdgeNumber = 38;

            // Act
            var rootNode = _radixTree.GetRoot();
            int? actualEdgeNumber = rootNode.GetNumberOfEdges();

            // Assert
            Assert.AreEqual(expectedEdgeNumber, actualEdgeNumber);
        }

        [Test]
        public void Test_RadixTreeDatabaseService_PrintsContent()
        {
            // Get the root node
            var rootNode = _radixTree.GetRoot();

            // Print the content of the root node
            var numEdges = rootNode.GetNumberOfEdges();
            Assert.AreEqual(38, numEdges);

            // Print the content of each edge of the root node
            for (int i = 0; i < numEdges; i++)
            {
                var edge = rootNode.GetNextEdge();
                Assert.NotNull(edge);
                var isWord = edge.IsWord();
                var isLeaf = edge.IsLeaf();
                var isLowercase = edge.IsLowerCase();
                var str = edge.GetString();

                Console.WriteLine($"Edge {i + 1}:");
                Console.WriteLine($"\tString: {str}");
                Console.WriteLine($"\tIs Leaf: {isLeaf}");
                Console.WriteLine($"\tIs Word: {isWord}");
                Console.WriteLine($"\tNext Node:");
                Console.WriteLine($"\t\tNumber of Edges: {edge.GetNode().GetNumberOfEdges()}");
            }
        }

        [Test]
        public void TestTotalBytes()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            _radixTree.PrintTotalBytes();
            string result = sw.ToString().Trim();
            Assert.AreEqual("Total number of bytes: 30265006", result);
        }

        [Test]
        public void TestFirstNBytes()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            _radixTree.PrintFirstNBytes(100);
            string result = sw.ToString().Trim();
            Assert.AreEqual("26-01-27-DB-00-00-00-81-61-E2-00-00-00-81-62-6F-01-00-00-81-63-C6-01-00-00-81-64-49-02-00-00-81-65-95-02-00-00-81-66-02-03-00-00-81-67-5E-03-00-00-81-68-C0-03-00-00-81-69-E0-03-00-00-81-6A-3E-04-00-00-81-6B-5F-04-00-00-81-6C-81-04-00-00-81-6D-C3-04-00-00-81-6E-1B-05-00-00-81-6F-6F-05-00-00-81-70-E5", result);
        }

        [Test]
        public void TestRootNode()
        {
            RadixTreeNode rootNode = _radixTree.GetRoot();
            Assert.IsNotNull(rootNode);
            Assert.AreEqual(38, rootNode.GetNumberOfEdges());
        }
    }
}