using ARLeF.Struments.Components.CoretorOrtografic.Core.KeyValueDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    public class MockKeyValueDatabase : IKeyValueDatabase
    {
        public Task<string> GetByKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}
