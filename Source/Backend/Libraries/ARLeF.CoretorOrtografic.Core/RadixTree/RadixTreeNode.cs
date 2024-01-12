//using System;
//using System.IO;

//namespace ARLeF.CoretorOrtografic.Core.RadixTree
//{
//    public class RadixTreeNode
//    {
//        private const int NTree = 0;
//        private const int NPos = 1;
//        private const int NnEdge = 2;
//        private const int NNextPos = 3;
//        private const int NNextNum = 4;

//        private readonly byte[] _tree;
//        private readonly int _position;
//        private readonly int _numberOfEdges;
//        private int _nextEdgePosition;
//        private int _nextEdgeNumber;

//        public RadixTreeNode(int position, byte[] tree)
//        {
//            _tree = tree;
//            _position = position;
//            _numberOfEdges = tree[position];
//            _nextEdgePosition = position + 1;
//            _nextEdgeNumber = 0;
//        }

//        public int GetNumberOfEdges()
//        {
//            return _numberOfEdges;
//        }

//        public RadixTreeEdge GetNextEdge()
//        {
//            if (_nextEdgeNumber >= _numberOfEdges)
//            {
//                _nextEdgeNumber = 0;
//                _nextEdgePosition = 1;
//                return null;
//            }

//            RadixTreeEdge edge = new RadixTreeEdge(_nextEdgePosition, _tree);
//            _nextEdgePosition += edge.GetDimension();
//            _nextEdgeNumber++;

//            return edge;
//        }

//        public RadixTreeNode Copy()
//        {
//            return new RadixTreeNode(_position, _tree);
//        }
//    }
//}
