using System;
using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;

namespace ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader
{
    public interface IRadixTreeReader
    {
        RadixTreeNode GetRootNode();
    }
}
