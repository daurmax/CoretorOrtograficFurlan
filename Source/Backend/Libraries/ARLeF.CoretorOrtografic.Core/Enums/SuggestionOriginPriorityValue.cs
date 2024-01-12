using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Core.Enums
{
    public enum SuggestionOriginPriorityValue
    {
        UserException = 1000,
        Same = 400,
        UserDictionary = 350,
        SystemErrors = 300
    }
}
