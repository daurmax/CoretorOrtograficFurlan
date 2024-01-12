using ARLeF.CoretorOrtografic.Core.RadixTree;
using ARLeF.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ARLeF.CoretorOrtografic.Infrastructure.RadixTreeDatabase
{
    public class RadixTreeDatabaseService
    {
        private byte[] data;

        public RadixTreeDatabaseService()
        {
            using (var fh = new FileStream(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH, FileMode.Open, FileAccess.Read))
            {
                data = new byte[fh.Length];
                fh.Read(data, 0, data.Length);
            }
        }

        public RadixTreeNode GetRoot()
        {
            return new RadixTreeNode(0, data);
        }

        public string PrintFirstNBytesAsHex(int n)
        {
            var retval = Convert.ToHexString(data.Take(n).ToArray());
            return retval;
        }
    }
}