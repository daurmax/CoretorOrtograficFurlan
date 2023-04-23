using System;
using System.IO;
using System.Text;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTree
{
    public class RadixTree
    {
        private readonly string _file;
        private readonly byte[] _data;

        public RadixTree(string file)
        {
            _file = file;
            try
            {
                _data = File.ReadAllBytes(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                throw;
            }
        }

        public RadixTreeNode GetRoot()
        {
            return new RadixTreeNode(0, _data);
        }

        public void PrintFirstNBytes(int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                sb.AppendFormat("{0:x2}-", _data[i]);
            }
            Console.WriteLine(sb.ToString());
        }

        public void PrintTotalBytes()
        {
            Console.WriteLine($"Total number of bytes: {_data.Length}");
        }
    }

    public class RadixTreeNode
    {
        private readonly int _position;
        private readonly byte[] _tree;
        private readonly int _numberOfEdges;
        private int _nextEdgePosition;
        private int _nextEdgeNumber;

        public RadixTreeNode(int position, byte[] tree)
        {
            _position = position;
            _tree = tree;
            _numberOfEdges = tree[position];
            _nextEdgePosition = position + 1;
            _nextEdgeNumber = 0;
        }

        public int GetNumberOfEdges()
        {
            return _numberOfEdges;
        }

        public RadixTreeEdge GetNextEdge()
        {
            if (_nextEdgeNumber >= _numberOfEdges)
            {
                _nextEdgeNumber = 0;
                _nextEdgePosition = 1;
                return null;
            }

            RadixTreeEdge edge = new RadixTreeEdge(_nextEdgePosition, _tree);
            _nextEdgePosition += edge.GetDimension();
            _nextEdgeNumber++;

            return edge;
        }

        public RadixTreeNode Copy()
        {
            return new RadixTreeNode(_position, _tree);
        }
    }

    public class RadixTreeEdge
    {
        private const byte IsWordFlag = 128;
        private const byte CaseFlag = 64;
        private const byte IsLeafFlag = 32;
        private const byte NoFlags = unchecked((byte)~(128 | 64 | 32));

        private readonly int _position;
        private readonly byte[] _tree;
        private readonly byte _edgeHeader;

        public RadixTreeEdge(int position, byte[] tree)
        {
            _position = position;
            _tree = tree;
            _edgeHeader = tree[position];
        }

        public int IsWord()
        {
            return (_edgeHeader & IsWordFlag) != 0
                ? (_edgeHeader & CaseFlag) != 0
                    ? 2
                    : 1
                : 0;
        }

        public bool IsLowerCase()
        {
            return (_edgeHeader & CaseFlag) == 0;
        }

        public bool IsLeaf()
        {
            return (_edgeHeader & IsLeafFlag) != 0;
        }

        public int GetLengthString()
        {
            return _edgeHeader & NoFlags;
        }

        public string GetString()
        {
            int len = GetLengthString();
            return Encoding.ASCII.GetString(_tree, _position + 1, len);
        }

        public int GetDimension()
        {
            return 1 + GetLengthString() + (IsLeaf() ? 0 : 4);
        }

        public RadixTreeNode GetNode()
        {
            int nodePos = BitConverter.ToInt32(_tree, _position + 1 + GetLengthString());
            return new RadixTreeNode(_position + nodePos, _tree);
        }
    }
}