//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ARLeF.CoretorOrtografic.Core.RadixTree
//{
//    public class RadixTree
//    {
//        private readonly byte[] _data;
//        private readonly string _file;

//        public RadixTree(string file)
//        {
//            _file = file;
//            try
//            {
//                _data = File.ReadAllBytes(file);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error reading file: {ex.Message}");
//                throw;
//            }
//        }

//        public RadixTreeNode GetRoot()
//        {
//            return new RadixTreeNode(0, _data);
//        }

//        public void PrintFirstNBytes(int n)
//        {
//            if (_data == null || n <= 0) return;

//            for (int i = 0; i < n && i < _data.Length; i++)
//            {
//                Console.Write($"{_data[i]:X2}");
//                if (i < n - 1)
//                {
//                    Console.Write("-");
//                }
//            }
//            Console.WriteLine();
//        }

//        public void PrintTotalBytes()
//        {
//            if (_data == null) return;
//            Console.WriteLine($"Total number of bytes: {_data.Length}");
//        }
//    }
//}
