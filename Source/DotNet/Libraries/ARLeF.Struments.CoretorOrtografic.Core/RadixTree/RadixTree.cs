using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTree
{
    public class RadixTree
    {
        private readonly byte[] _data;
        private readonly string _file;

        public RadixTree(string file)
        {
            _file = file;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                _data = br.ReadBytes((int)fs.Length);
            }
        }

        public RadixTreeNode GetRoot()
        {
            return new RadixTreeNode(0, _data);
        }

        public void PrintFirstNBytes(int n)
        {
            using (var fs = new FileStream(_file, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[n];
                fs.Read(data, 0, n);
                Console.WriteLine(string.Join("-", Array.ConvertAll(data, b => $"{b:X2}")));
            }
        }

        public void PrintTotalBytes()
        {
            Console.WriteLine("Total number of bytes: " + _data.Length);
        }
    }
}
