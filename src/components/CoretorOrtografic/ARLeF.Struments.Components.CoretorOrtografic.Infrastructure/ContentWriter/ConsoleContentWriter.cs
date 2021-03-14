using ARLeF.Struments.Base.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.Output
{
    public class ConsoleContentWriter : IContentWriter
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }
}
