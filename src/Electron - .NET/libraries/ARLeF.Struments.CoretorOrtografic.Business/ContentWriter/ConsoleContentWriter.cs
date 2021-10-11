using ARLeF.Struments.CoretorOrtografic.Contracts.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Business.Output
{
    public class ConsoleContentWriter : IContentWriter
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }
}
