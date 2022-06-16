using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    public class BerkeleyDbKeyValueDatabase : IKeyValueDatabase
    {
        public Task<string> GetPhoneticSuggestions(string hash1, string hash2)
        {
            throw new NotImplementedException();
        }
    }
}
