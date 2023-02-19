using System;
using System.IO;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTree
{
    public class RadixTreeEdge
    {
        private int pos;
        private byte[] tree;

        private const byte IsWordFlag = 128;
        private const byte CaseFlag = 64;
        private const byte IsLeafFlag = 32;
        private const byte NoFlags = 0x1F;
        private const int EdgeHeadDim = 1;
        private const int OffsetDim = 4;

        public RadixTreeEdge(int pos, byte[] tree)
        {
            this.pos = pos;
            this.tree = tree;
        }

        public bool IsWord()
        {
            byte flags = tree[pos + EdgeHeadDim];
            if ((flags & IsWordFlag) == IsWordFlag)
            {
                return (flags & CaseFlag) == CaseFlag ? true : false;
            }
            return false;
        }
        public bool IsLowercase()
        {
            return (tree[pos + EdgeHeadDim] & CaseFlag) == 0;
        }
        public bool IsLeaf()
        {
            return (tree[pos + EdgeHeadDim] & IsLeafFlag) == IsLeafFlag;
        }

        public int GetLength()
        {
            return tree[pos + EdgeHeadDim] & NoFlags;
        }
        public string GetString()
        {
            int length = GetLength();
            return System.Text.Encoding.UTF8.GetString(tree, pos + EdgeHeadDim + 1, length);
        }
        public int GetDimension()
        {
            return EdgeHeadDim + GetLength() + (IsLeaf() ? 0 : OffsetDim);
        }
        public RadixTreeNode GetNode()
        {
            int nodePos = BitConverter.ToInt32(tree, pos + EdgeHeadDim + GetLength());
            return new RadixTreeNode(pos + nodePos, tree);
        }
    }
}
