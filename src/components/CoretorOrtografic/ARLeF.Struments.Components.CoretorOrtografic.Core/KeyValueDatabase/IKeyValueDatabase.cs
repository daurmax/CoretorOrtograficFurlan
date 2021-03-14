using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Core.KeyValueDatabase
{
    public interface IKeyValueDatabase
    {
        /// <summary>
        /// Retrive value by key
        /// </summary>
        /// <param name="id">key associated with a value</param>
        /// <returns>Value associated to a key</returns>
        Task<string> GetByKey(string key);
    }
}
