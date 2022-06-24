using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.KeyValueDatabase
{
    public class LiteDbKeyValueDatabase : IKeyValueDatabase
    {
        public string GetValueAsStringByKey(string key)
        {
            using (var db = new LiteDatabase(DictionaryFilePaths.LITEDB_WORDS_DATABASE_FILE_PATH))
            {
                var wordsCollection = db.GetCollection<BsonDocument>("words");
                var retrievedValue = wordsCollection.FindOne(Query.EQ("_id", key));
                return retrievedValue?.AsString;
            }
        }
    }
}
