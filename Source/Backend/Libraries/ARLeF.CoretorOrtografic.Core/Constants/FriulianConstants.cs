using System;
using System.Collections.Generic;

namespace ARLeF.CoretorOrtografic.Core.Constants
{
	public static class FriulianConstants
	{
		public static readonly string UNCOMMON_APOSTROPHES = "\u2018|\u2019|\u0091|\u0092";
		public static readonly string FRIULIAN_LETTERS = "a-zA-ZçàáèéìíòóùúâêîôûÂÊÎÔÛÇÀÁÈÉÌÍÒÓÙÚ";
		public static readonly string FRIULIAN_VOWELS = "aeiouAEIOUâêîôûÂÊÎÔÛ";
        public static readonly string WORD_LETTERS = "a-zA-ZµÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿ";

        public static readonly string SMALL_A_VARIANTS = @"'a|à|â|á";
		public static readonly string SMALL_E_VARIANTS = @"'e|è|ê|é";
		public static readonly string SMALL_I_VARIANTS = @"'i|ì|î|í";
		public static readonly string SMALL_O_VARIANTS = @"'o|ò|ô|ó";
		public static readonly string SMALL_U_VARIANTS = @"'u|ù|û|ú";

        public static Dictionary<char, int> VOWELS_A = new Dictionary<char, int>
{
    {'a', 1}, {'à', 1}, {'á', 1}, {'â', 1}
};

        public static Dictionary<char, int> VOWELS_E = new Dictionary<char, int>
{
    {'e', 1}, {'è', 1}, {'é', 1}, {'ê', 1}
};

        public static Dictionary<char, int> VOWELS_I = new Dictionary<char, int>
{
    {'i', 1}, {'ì', 1}, {'í', 1}, {'î', 1}, {'j', 1}
};

        public static Dictionary<char, int> VOWELS_O = new Dictionary<char, int>
{
    {'o', 1}, {'ò', 1}, {'ó', 1}, {'ô', 1}
};

        public static Dictionary<char, int> VOWELS_U = new Dictionary<char, int>
{
    {'u', 1}, {'ù', 1}, {'ú', 1}, {'û', 1}
};
    }
}

