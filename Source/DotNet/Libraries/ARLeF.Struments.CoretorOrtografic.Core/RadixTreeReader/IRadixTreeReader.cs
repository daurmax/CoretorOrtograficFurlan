using System;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTree;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader
{
    public interface IRadixTreeReader
    {
        RadixTreeNode RootNode { get; }
    }
}
