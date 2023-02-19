using System;
using System.Diagnostics;
using System.IO;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using NUnit.Framework;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTree
{
    public class RadixTreeNode
    {
        private string _radixFilePath;
        private int _edgePosition;

        public RadixTreeNode(string radixTreeFilePath, int edgePosition)
        {
            _radixFilePath = radixTreeFilePath;
            _edgePosition = edgePosition;
        }

        //public RadixTreeNode GetNextEdge()
        //{
        //    if (EdgesNumber <= NextNumber)
        //    {
        //        return new RadixTreeNode();
        //    }
        //}

        public int EdgePosition
        {
            get
            {
                return _edgePosition;
            }
        }
        public byte EdgesNumber
        {
            get
            {
                return RetrieveEdgeByte(_edgePosition);
            }
        }
        public int NextPosition
        {
            get
            {
                return _edgePosition + 1;
            }
        }
        public byte NextNumber
        {
            get
            {
                return 0;
            }
        }

        private byte RetrieveEdgeByte(int position)
        {
            FileStream radixTreeFileStream = File.OpenRead(_radixFilePath);

            using (BinaryReader reader = new BinaryReader(radixTreeFileStream))
            {
                reader.BaseStream.Seek(_edgePosition + (int)position, SeekOrigin.Begin);

                //Console.WriteLine($"Byte array position: [{reader.BaseStream.Position}]");
                //Console.WriteLine($"Byte array value: [{reader.ReadByte()}]");

                //reader.Read(test, i, 10);
                return reader.ReadByte();
            }
        }
    }

    public class RadixTreeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadWordsRadixTreeTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            var rootNode = new RadixTreeNode(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH, 0);

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
