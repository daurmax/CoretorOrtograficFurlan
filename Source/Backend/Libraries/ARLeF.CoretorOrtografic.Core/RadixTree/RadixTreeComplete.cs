using System;
using System.IO;
using System.Text;

namespace ARLeF.CoretorOrtografic.Core.RadixTree
{
    public class RadixTree
    {
        private readonly string _file;
        private readonly byte[] _data;

        public RadixTree(string file)
        {
            //Console.WriteLine("C# - Initializing RadixTree"); // Debugging statement
            _file = file;
            try
            {
                _data = File.ReadAllBytes(file);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public RadixTreeNode GetRoot()
        {
            //Console.WriteLine("C# - Called GetRoot"); // Debugging statement
            return new RadixTreeNode(0, _data);
        }

        public void PrintFirstNBytes(int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                sb.AppendFormat("{0:X2}", _data[i]);
                if (i < n - 1)
                {
                    sb.Append('-');
                }
            }
            //Console.WriteLine(sb.ToString()); // Debugging statement
        }

        public void PrintTotalBytes()
        {
            //Console.WriteLine($"Total number of bytes: {_data.Length}"); // Debugging statement
        }
    }

    public class RadixTreeNode
    {
        public readonly int _position;
        private readonly byte[] _tree;
        private readonly int _numberOfEdges;
        private int _nextEdgePosition;
        private int _nextEdgeNumber;

        public RadixTreeNode(int position, byte[] tree)
        {
            //Console.WriteLine($"C# - Initializing RadixTreeNode at position {position}"); // Debugging statement
            _position = position;
            _tree = tree;

            if (_position >= 0 && _position < _tree.Length)
            {
                _numberOfEdges = tree[position];
                _nextEdgePosition = position + 1;
            }
            else
            {
                _numberOfEdges = 0;
                _nextEdgePosition = -1;
            }
            _nextEdgeNumber = 1;
        }

        public int GetNumberOfEdges()
        {
            //Console.WriteLine($"C# - Called GetNumberOfEdges, returning: {_numberOfEdges}"); // Debugging statement
            return _numberOfEdges;
        }

        public RadixTreeEdge GetNextEdge()
        {
            //Console.WriteLine($"C# - Called GetNextEdge by node with position {_position}"); // Debugging statement

            if (_nextEdgeNumber > GetNumberOfEdges() || _nextEdgePosition < 0 || _nextEdgePosition >= _tree.Length)
            {
                _nextEdgeNumber = 0;
                _nextEdgePosition = 1;
                return null;
            }

            _nextEdgeNumber++; // Increment the edge number before creating a new edge

            RadixTreeEdge edge = new RadixTreeEdge(_nextEdgePosition, _tree);
            //Console.WriteLine($"C# - GetNextEdge created edge at position {_nextEdgePosition} with edge number {_nextEdgeNumber - 1}"); // Debugging statement

            _nextEdgePosition += edge.GetDimension();

            return edge;
        }

        public RadixTreeNode Copy()
        {
            //Console.WriteLine("Called 'Copy'"); // Debugging statement
            return new RadixTreeNode(_position, _tree);
        }
    }

    public class RadixTreeEdge
    {
        private const byte IsWordFlag = 128;
        private const byte CaseFlag = 64;
        private const byte IsLeafFlag = 32;
        private const byte NoFlags = unchecked((byte)~(128 | 64 | 32));
        private const byte EdgeHeadDimension = 1;

        private readonly int _position;
        private readonly byte[] _tree;
        private readonly byte _edgeHeader;

        public int Position => _position;

        public RadixTreeEdge(int position, byte[] tree)
        {
            //Console.WriteLine($"C# - Initializing RadixTreeEdge at position {position}"); // Debugging statement
            _position = position;
            _tree = tree;
            _edgeHeader = tree[position];
        }

        public int IsWord()
        {
            string edgeString = GetString();
            //Console.WriteLine($"C# - Called IsWord on string: {edgeString}"); // Debugging statement

            bool isWordFlagSet = (_edgeHeader & IsWordFlag) != 0;
            //Console.WriteLine($"C# - IsWordFlag set: {isWordFlagSet}");

            bool caseFlagSet = (_edgeHeader & CaseFlag) != 0;
            //Console.WriteLine($"C# - CaseFlag set: {caseFlagSet}");

            int result = isWordFlagSet
                ? (caseFlagSet ? 2 : 1)
                : 0;

            //Console.WriteLine($"C# - Final value before returning: {result}"); // Added debugging statement

            return result;
        }

        public bool IsLowerCase()
        {
            //Console.WriteLine("C# - Called 'IsLowerCase'"); // Debugging statement

            bool isLowerCase = (_edgeHeader & CaseFlag) != 0;

            //Console.WriteLine($"C# - Final value before returning: {isLowerCase}"); // Added debugging statement

            return isLowerCase;
        }

        public bool IsLeaf()
        {
            bool isLeaf = (_edgeHeader & IsLeafFlag) != 0;
            //Console.WriteLine($"C# - IsLeaf called on edge at position {_position}, value: {isLeaf}");
            return isLeaf;
        }

        public int GetLengthOfString()
        {
            return _edgeHeader & NoFlags;
        }

        public string GetString()
        {
            string edgeString = Encoding.Latin1.GetString(_tree, _position + 1, GetLengthOfString());

            return edgeString;
        }

        public int GetDimension()
        {
            return 1 + GetLengthOfString() + (IsLeaf() ? 0 : 4);
        }

        public RadixTreeNode GetNode()
        {
            if (IsLeaf())
            {
                return null;
            }
            int edgeHeadDim = EdgeHeadDimension;
            int lenString = GetLengthOfString();
            int nodePositionOffset = _position + edgeHeadDim + lenString;
            int nodePosition = BitConverter.ToInt32(_tree, nodePositionOffset);
            int newNodePosition = _position + nodePosition;

            return new RadixTreeNode(newNodePosition, _tree);
        }
    }
}