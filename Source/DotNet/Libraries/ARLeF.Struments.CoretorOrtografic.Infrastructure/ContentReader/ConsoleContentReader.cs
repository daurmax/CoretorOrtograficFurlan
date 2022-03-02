using ARLeF.Struments.CoretorOrtografic.Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.ContentReader
{
    public class ConsoleContentReader : IContentReader
    {
        public string Read(object source = null)
        {
            return Console.ReadLine();
        }
    }
}
