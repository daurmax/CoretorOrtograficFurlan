using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Infrastructure.RadixTreeDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTreeDatabase
{
    public class RadixTreeTests
    {
        private ARLeF.Struments.CoretorOrtografic.Core.RadixTree.RadixTree _radixTree;

        [SetUp]
        public void Setup()
        {
            _radixTree = new ARLeF.Struments.CoretorOrtografic.Core.RadixTree.RadixTree(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH);
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
    }
}