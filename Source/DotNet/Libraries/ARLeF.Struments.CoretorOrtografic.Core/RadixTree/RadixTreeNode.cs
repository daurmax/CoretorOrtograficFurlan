using System;
using System.IO;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTree
{
    public class RadixTreeNode
    {
        private readonly byte[] _tree;
        public int Pos { get; }
        private const int NNEDGE = 2;
        private const int NNEXT_POS = 3;
        private const int NNEXT_NUM = 4;
        public int? NumEdges => GetNumEdges();
        public int NextPos { get; set; } = 1;
        public int NextNum { get; set; } = 0;

        public RadixTreeNode(int pos, byte[] tree)
        {
            _tree = tree;
            Pos = pos;
            NextPos = 1;
            NextNum = 0;
        }

        public int? GetNumEdges()
        {
            try
            {
                return _tree[Pos];
            }
            catch (IndexOutOfRangeException ex)
            {
                return null;
            }
        }

        public RadixTreeEdge GetNextEdge()
        {
            if (NumEdges <= NextNum)
            {
                NextNum = 0;
                NextPos = 1;
                return null;
            }

            NextNum++;
            var edge = new RadixTreeEdge(NextPos, _tree);
            NextPos += edge.GetDimension();
            return edge;
        }

        public RadixTreeNode Copy()
        {
            return new RadixTreeNode(Pos, _tree);
        }
    }
}
