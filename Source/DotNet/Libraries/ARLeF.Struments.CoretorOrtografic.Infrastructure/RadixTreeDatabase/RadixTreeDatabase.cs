using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.RadixTreeDatabase
{
    public class RadixTreeDatabaseService
    {
        private byte[] data;

        private const int NodeHeadDim = 1;

        public RadixTreeDatabaseService()
        {
            using (FileStream fileStream = new FileStream(DictionaryFilePaths.SQLITE_WORDS_DATABASE_FILE_PATH, FileMode.Open, FileAccess.Read))
            {
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);
            }
        }

        public RadixTreeNode GetRoot()
        {
            return new RadixTreeNode(0, data);
        }
    }
}