using CoretorOrtografic.Base.Abstractions.Development;
using CoretorOrtografic.Base.Abstractions.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoretorOrtografic.Components.Infrastructure.Development
{
    public class TodayWriter : IDateWriter
    {
        private IOutput _output;
        public TodayWriter(IOutput output)
        {
            this._output = output;
        }

        public void WriteDate()
        {
            this._output.Write(DateTime.Today.ToShortDateString());
        }
    }
}
