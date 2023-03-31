using System;
using System.IO;
using System.Text;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTree
{
    public class RadixTreeEdge
    {
        private const byte IS_WORD_FLAG = 128;
        private const byte CASE_FLAG = 64;
        private const byte IS_LEAF_FLAG = 32;
        private const byte NO_FLAGS = unchecked((byte)~(IS_WORD_FLAG | CASE_FLAG | IS_LEAF_FLAG));
        private const byte EDGE_HEAD_DIM = 1;
        private const byte OFFSET_DIM = 4;

        private int _pos;
        private byte[] _tree;

        public RadixTreeEdge(int pos, byte[] tree)
        {
            _tree = tree;
            _pos = pos;
        }

        //public bool IsWord()
        //{
        //    byte flags = _tree[_pos];
        //    //return (flags & IS_WORD_FLAG) != 0;
        //    return flags == IS_WORD_FLAG;
        //}

        public int IsWord()
        {
            byte flags = _tree[_pos];

            if ((flags & IS_WORD_FLAG) != 0)
            {
                if ((flags & CASE_FLAG) != 0)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool IsLowerCase()
        {
            byte flags = _tree[_pos];
            return (flags & CASE_FLAG) == 0;
        }

        public bool IsLeaf()
        {
            byte flags = _tree[_pos];
            return (flags & IS_LEAF_FLAG) != 0;
        }

        public int GetLenString()
        {
            byte flags = _tree[_pos];
            return flags & NO_FLAGS;
        }

        public string GetString()
        {
            int len = GetLenString();
            byte[] bytes = new byte[len];
            Array.Copy(_tree, _pos + 1, bytes, 0, len);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public int GetDimension()
        {
            return EDGE_HEAD_DIM + GetLenString() + (IsLeaf() ? 0 : OFFSET_DIM);
        }

        public RadixTreeNode GetNode()
        {
            int nodePos = BitConverter.ToInt32(_tree, _pos + EDGE_HEAD_DIM + GetLenString());
            return new RadixTreeNode(_pos + nodePos, _tree);
        }
    }
}
