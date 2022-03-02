using System;
using System.IO;

namespace ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree
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
}
