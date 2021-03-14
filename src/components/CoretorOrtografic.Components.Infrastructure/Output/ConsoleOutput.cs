using CoretorOrtografic.Base.Abstractions.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoretorOrtografic.Components.Infrastructure.Output
{
    public class ConsoleOutput : IOutput
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }
}
