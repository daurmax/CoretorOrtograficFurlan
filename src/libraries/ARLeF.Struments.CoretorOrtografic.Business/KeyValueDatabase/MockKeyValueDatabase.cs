using ARLeF.Struments.CoretorOrtografic.Contracts.KeyValueDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Business.KeyValueDatabase
{
    public class MockKeyValueDatabase : IKeyValueDatabase
    {
        public Task<string> GetSuggestionsByKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}
