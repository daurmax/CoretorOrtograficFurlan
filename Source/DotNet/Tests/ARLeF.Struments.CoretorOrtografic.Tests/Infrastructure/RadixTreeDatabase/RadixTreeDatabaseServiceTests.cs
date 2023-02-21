//using ARLeF.Struments.CoretorOrtografic.Infrastructure.RadixTreeDatabase;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTreeDatabase
//{
//    public class RadixTreeDatabaseServiceTests
//    {
//        private RadixTreeDatabaseService _radixTreeDatabaseService;

//        [SetUp]
//        public void Setup()
//        {
//            _radixTreeDatabaseService = new RadixTreeDatabaseService();
//        }

//        [Test]
//        public void GetRoot_Returns_RadixTreeNode_With_Correct_EdgeNumber()
//        {
//            // Arrange
//            int expectedEdgeNumber = 38;

//            // Act
//            var rootNode = _radixTreeDatabaseService.GetRoot();
//            int actualEdgeNumber = rootNode.GetNumEdges();

//            // Assert
//            Assert.AreEqual(expectedEdgeNumber, actualEdgeNumber);
//        }

//        [Test]
//        public void PrintFirstNBytesAsHex_Returns_First_100_Bytes_As_Hex()
//        {
//            // Arrange

//            // Act
//            _radixTreeDatabaseService.PrintFirstNBytesAsHex(100);

//            // Assert
//            Assert.True(true);
//        }

//        [Test]
//        public void Test_RadixTreeDatabaseService_PrintsContent()
//        {
//            // Get the root node
//            var rootNode = _radixTreeDatabaseService.GetRoot();

//            // Print the content of the root node
//            var numEdges = rootNode.GetNumEdges();
//            Assert.AreEqual(38, numEdges);

//            // Print the content of each edge of the root node
//            for (int i = 0; i < numEdges; i++)
//            {
//                var edge = rootNode.GetNextEdge();
//                Assert.NotNull(edge);
//                var isWord = edge.IsWord();
//                var isLeaf = edge.IsLeaf();
//                var isLowercase = edge.IsLowercase();
//                var str = edge.GetString();

//                Console.WriteLine($"Edge {i + 1}:");
//                Console.WriteLine($"\tString: {str}");
//                Console.WriteLine($"\tIs Leaf: {isLeaf}");
//                Console.WriteLine($"\tIs Word: {isWord}");
//                Console.WriteLine($"\tNext Node:");
//                Console.WriteLine($"\t\tNumber of Edges: {edge.GetNode().GetNumEdges()}");
//            }
//        }
//    }
//}