using System;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.RadixTreeReader
{
    public class InMemoryRadixTreeReader : IRadixTreeReader
    {
        RadixTreeNode IRadixTreeReader.RootNode => new RadixTreeNode(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH, 0);

        public InMemoryRadixTreeReader() { }
    }
}
