using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    public class MockKeyValueDatabase : IKeyValueDatabase
    {
        public Task<string> GetSuggestionsByKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}
