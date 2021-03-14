using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Base.Core.Input
{
    public interface IContentReader
    {
        string Read(object source = null);
    }
}
