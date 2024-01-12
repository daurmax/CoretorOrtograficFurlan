using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="System.String"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a copy of this string with the first character of the input string converted to uppercase.
        /// If the first character is an apostrophe, the second character is converted to uppercase.
        /// </summary>
        /// <param name="word">The input string to be converted.</param>
        /// <returns>A copy of this string with the first character (or the second character after an apostrophe) converted to uppercase.</returns>
        /// <remarks>
        /// If the input string is null or empty, the method returns the original string without any modifications.
        /// If the input string starts with an apostrophe and contains only one character, the method returns the original string without any modifications.
        /// </remarks>
        public static string ToFirstCharacterUpper(this string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }

            if (word[0] == '\'')
            {
                if (word.Length == 1)
                {
                    return word;
                }
                string firstLetter = word.Substring(1, 1);
                return "'" + firstLetter.ToUpper() + word.Substring(2);
            }
            else
            {
                string firstLetter = word.Substring(0, 1);
                return firstLetter.ToUpper() + word.Substring(1);
            }
        }
    }
}
