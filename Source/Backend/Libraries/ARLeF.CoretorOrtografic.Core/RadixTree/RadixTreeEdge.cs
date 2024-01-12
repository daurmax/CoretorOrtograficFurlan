//using System;
//using System.IO;
//using System.Text;

//namespace ARLeF.CoretorOrtografic.Core.RadixTree
//{
//    public class RadixTreeEdge
//    {
//        private const byte IsWordFlag = 128;
//        private const byte CaseFlag = 64;
//        private const byte IsLeafFlag = 32;
//        private const byte NoFlags = unchecked((byte)~(128 | 64 | 32));

//        private readonly int _position;
//        private readonly byte[] _tree;
//        private readonly byte _edgeHeader;

//        public RadixTreeEdge(int position, byte[] tree)
//        {
//            _position = position;
//            _tree = tree;
//            _edgeHeader = tree[position];
//        }

//        public int IsWord()
//        {
//            return (_edgeHeader & IsWordFlag) != 0
//                ? (_edgeHeader & CaseFlag) != 0
//                    ? 2
//                    : 1
//                : 0;
//        }

//        public bool IsLowerCase()
//        {
//            return (_edgeHeader & CaseFlag) == 0;
//        }

//        public bool IsLeaf()
//        {
//            return (_edgeHeader & IsLeafFlag) != 0;
//        }

//        public int GetLengthString()
//        {
//            return _edgeHeader & NoFlags;
//        }

//        public string GetString()
//        {
//            int len = GetLengthString();
//            return System.Text.Encoding.ASCII.GetString(_tree, _position + 1, len);
//        }

//        public int GetDimension()
//        {
//            return 1 + GetLengthString() + (IsLeaf() ? 0 : 4);
//        }

//        public RadixTreeNode GetNode()
//        {
//            int nodePos = BitConverter.ToInt32(_tree, _position + 1 + GetLengthString());
//            return new RadixTreeNode(_position + nodePos, _tree);
//        }
//    }
//}
