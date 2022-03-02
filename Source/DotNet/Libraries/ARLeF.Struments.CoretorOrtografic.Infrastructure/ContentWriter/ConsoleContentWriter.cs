using ARLeF.Struments.CoretorOrtografic.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.Output
{
    public class ConsoleContentWriter : IContentWriter
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }
}
