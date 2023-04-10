using ARLeF.Struments.CoretorOrtografic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase
{
    public interface IKeyValueDatabase
    {
        string FindInSystemDatabase(string key);
        int? FindInFrequenciesDatabase(string key);
    }
}
