using System;
using System.IO;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTree
{
    public class RadixTreeNode
    {
        private byte[] tree;
        private int pos;
        private int numEdges;
        private int nextPos;
        private int nextNum;

        private const int NTree = 0;
        private const int NPos = 1;
        private const int NNumEdges = 2;
        private const int NNextPos = 3;
        private const int NNextNum = 4;

        public RadixTreeNode(int pos, byte[] tree)
        {
            this.tree = tree;
            this.pos = pos;
            numEdges = tree[pos];
            nextPos = pos + 1;
            nextNum = 0;
        }

        public int GetNumEdges()
        {
            return numEdges;
        }

        public RadixTreeEdge GetNextEdge()
        {
            if (numEdges <= nextNum)
            {
                nextNum = 0;
                nextPos = pos + 1;
                return null;
            }

            nextNum++;
            RadixTreeEdge edge = new RadixTreeEdge(nextPos, tree);
            nextPos += edge.GetDimension();
            return edge;
        }

        public RadixTreeNode Copy()
        {
            return new RadixTreeNode(pos, tree);
        }
    }
}
