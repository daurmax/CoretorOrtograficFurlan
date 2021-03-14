using ARLeF.Struments.Base.Core.Development;
using ARLeF.Struments.Base.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Base.Infrastructure.Development
{
    public class TodayWriter : IDateWriter
    {
        private readonly IContentWriter _output;
        public TodayWriter(IContentWriter output)
        {
            this._output = output;
        }

        public void WriteDate()
        {
            this._output.Write(DateTime.Today.ToShortDateString());
        }
    }
}
