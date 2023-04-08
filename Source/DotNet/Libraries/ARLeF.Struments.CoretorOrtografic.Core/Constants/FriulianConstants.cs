using System;
namespace ARLeF.Struments.CoretorOrtografic.Core.Constants
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
	}
}

