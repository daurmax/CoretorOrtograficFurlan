using System;
using System.Text;
using System.Text.RegularExpressions;
using ARLeF.Struments.CoretorOrtografic.Core.Constants;

namespace ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm
{
	public static class FurlanPhoneticAlgorithmModel
	{
		public static (string, string) GetPhoneticHashesByWord(string word)
        {
            return GetPhoneticHashesByOriginal(PrepareOriginalWord(word));
        }

        private static string PrepareOriginalWord(string original)
        {
            // Replace uncommon apostrophes with '
            original = Regex.Replace(original, FriulianConstants.UNCOMMON_APOSTROPHES, "'");

            // Replace "e " with "'"
            original = Regex.Replace(original, "e ", "'");

            // Remove all spaces from word (should never happen at this point but still)
            original = Regex.Replace(original, " ", "");

            // Remove double letters
            var strResult = new StringBuilder();
            foreach (var element in original.ToCharArray())
            {
                if (strResult.Length == 0 || strResult[strResult.Length - 1] != element)
                    strResult.Append(element);
            }
            original = strResult.ToString();

            // Make the string lowercase
            original = original.ToLower();

            // Replace "h'" with "K"
            original = Regex.Replace(original, "h'", "K");

            // Replace Friulian vowels with Latin vowels
            original = Regex.Replace(original, FriulianConstants.SMALL_A_VARIANTS, "a");
            original = Regex.Replace(original, FriulianConstants.SMALL_E_VARIANTS, "e");
            original = Regex.Replace(original, FriulianConstants.SMALL_I_VARIANTS, "i");
            original = Regex.Replace(original, FriulianConstants.SMALL_O_VARIANTS, "o");
            original = Regex.Replace(original, FriulianConstants.SMALL_U_VARIANTS, "u");

            original = Regex.Replace(original, "çi", "ci");
            original = Regex.Replace(original, "çe", "ce");

            original = Regex.Replace(original, "ds$", "ts"); // Only at the end
            original = Regex.Replace(original, "sci", "ssi");
            original = Regex.Replace(original, "sce", "se");

            original = Regex.Replace(original, "çi", "ci");
            original = Regex.Replace(original, "çe", "ce");

            original = Regex.Replace(original, " ", "");

            original = Regex.Replace(original, "w", "");
            original = Regex.Replace(original, "y", "");
            original = Regex.Replace(original, "x", "");

            original = Regex.Replace(original, "^che", "chi"); // Only at the beginning

            original = Regex.Replace(original, "h", "");

            original = Regex.Replace(original, "leng", "X");
            original = Regex.Replace(original, "lingu", "X");

            original = Regex.Replace(original, "amentri", "O");
            original = Regex.Replace(original, "ementri", "O");
            original = Regex.Replace(original, "amenti", "O");
            original = Regex.Replace(original, "ementi", "O");

            original = Regex.Replace(original, "uintri", "W");
            original = Regex.Replace(original, "ontra", "W");

            original = Regex.Replace(original, "ur", "Y");
            original = Regex.Replace(original, "uar", "Y");
            original = Regex.Replace(original, "or", "Y");

            original = Regex.Replace(original, "^'s", "s"); // Only at the beginning
            original = Regex.Replace(original, "^'n", "n"); // Only at the beginning

            original = Regex.Replace(original, "ins$", "1"); // Only at the end
            original = Regex.Replace(original, "in$", "1"); // Only at the end
            original = Regex.Replace(original, "ims$", "1"); // Only at the end
            original = Regex.Replace(original, "im$", "1"); // Only at the end
            original = Regex.Replace(original, "gns$", "1"); // Only at the end
            original = Regex.Replace(original, "gn$", "1"); // Only at the end

            original = Regex.Replace(original, "mn", "5");
            original = Regex.Replace(original, "nm", "5");
            original = Regex.Replace(original, "[mn]", "5");

            original = Regex.Replace(original, "er", "2");
            original = Regex.Replace(original, "ar", "2");

            original = Regex.Replace(original, "b$", "3");
            original = Regex.Replace(original, "p$", "3");

            original = Regex.Replace(original, "v$", "4");
            original = Regex.Replace(original, "f$", "4");

            return original;
        }

        private static (string, string) GetPhoneticHashesByOriginal(string original)
        {
            string firstHash = original;
            string secondHash = original;

            firstHash = Regex.Replace(firstHash, "'c", "A");
            firstHash = Regex.Replace(firstHash, "c[ji]us$", "A");
            firstHash = Regex.Replace(firstHash, "c[ji]u$", "A");
            firstHash = Regex.Replace(firstHash, "c'", "A");
            firstHash = Regex.Replace(firstHash, "ti", "A");
            firstHash = Regex.Replace(firstHash, "ci", "A");
            firstHash = Regex.Replace(firstHash, "si", "A");
            firstHash = Regex.Replace(firstHash, "zs", "A");
            firstHash = Regex.Replace(firstHash, "zi", "A");
            firstHash = Regex.Replace(firstHash, "cj", "A");
            firstHash = Regex.Replace(firstHash, "çs", "A");
            firstHash = Regex.Replace(firstHash, "tz", "A");
            firstHash = Regex.Replace(firstHash, "z", "A");
            firstHash = Regex.Replace(firstHash, "ç", "A");
            firstHash = Regex.Replace(firstHash, "c", "A");
            firstHash = Regex.Replace(firstHash, "q", "A");
            firstHash = Regex.Replace(firstHash, "k", "A");
            firstHash = Regex.Replace(firstHash, "ts", "A");
            firstHash = Regex.Replace(firstHash, "s", "A");

            secondHash = Regex.Replace(secondHash, "c$", "0");
            secondHash = Regex.Replace(secondHash, "g$", "0");

            secondHash = Regex.Replace(secondHash, "bs$", "s");
            secondHash = Regex.Replace(secondHash, "cs$", "s");
            secondHash = Regex.Replace(secondHash, "fs$", "s");
            secondHash = Regex.Replace(secondHash, "gs$", "s");
            secondHash = Regex.Replace(secondHash, "ps$", "s");
            secondHash = Regex.Replace(secondHash, "vs$", "s");

            secondHash = Regex.Replace(secondHash, "di(?=.)", "E");
            secondHash = Regex.Replace(secondHash, "gji", "E");
            secondHash = Regex.Replace(secondHash, "gi", "E");
            secondHash = Regex.Replace(secondHash, "gj", "E");
            secondHash = Regex.Replace(secondHash, "g", "E");

            secondHash = Regex.Replace(secondHash, "ts", "E");
            secondHash = Regex.Replace(secondHash, "s", "E");
            secondHash = Regex.Replace(secondHash, "zi", "E");
            secondHash = Regex.Replace(secondHash, "z", "E");

            firstHash = Regex.Replace(firstHash, "j", "i");
            secondHash = Regex.Replace(secondHash, "j", "i");

            // Remove double "i" in both hashes (WIP - NOT WORKING)
            var strResult = new StringBuilder();
            foreach (var element in firstHash.ToCharArray())
            {
                if ((element == 'i') && (strResult.Length == 0 || strResult[strResult.Length - 1] != element))
                    strResult.Append(element);
            }
            original = strResult.ToString();
        }
    }

    // Original algorithm by Perl-based COF
    /*
    sub phalg_furlan
    {
        my $original = $_ [0];
        my $primo;
        my $secondo;
        my $word = '';

        my $slash_W = qr/[^$FUR_LETTERS$FUR_APOSTROPHS]/;

        $original =~ s/[$FUR_APOSTROPHS]/'/g;
        $original =~ s/e /'/;
        $original =~ s/\s+|\$slash_W+//g;

        $original =~ tr/\0-\377//s;

        $original = lc_word($original);

        $original =~ s/h'/K/;

        $original =~ s/à/a/g;
        $original =~ s/â/a/g;
        $original =~ s/á/a/g;
        $original =~ s/'a/a/g;

        $original =~ s/è/e/g;
        $original =~ s/ê/e/g;
        $original =~ s/é/e/g;
        $original =~ s/'e/e/g;

        $original =~ s/ì/i/g;
        $original =~ s/î/i/g;
        $original =~ s/í/i/g;
        $original =~ s/'i/i/g;

        $original =~ s/ò/o/g;
        $original =~ s/ô/o/g;
        $original =~ s/ó/o/g;
        $original =~ s/'o/o/g;

        $original =~ s/ù/u/g;
        $original =~ s/û/u/g;
        $original =~ s/ú/u/g;
        $original =~ s/'u/u/g;

        $original =~ s/çi/ci/g;
        $original =~ s/çe/ce/g;

        $original =~ s/ds$/ts/;
        $original =~ s/sci/ssi/g;
        $original =~ s/sce/se/g;

        $original =~ tr/\0-\377//s;

        $original =~ s/w//g;
        $original =~ s/y//g;
        $original =~ s/x//g;

        $original =~ s/^che/chi/g;

        $original =~ s/h//g;

        $original =~ s/leng/X/g;
        $original =~ s/lingu/X/g;

        $original =~ s/amentri/O/g;
        $original =~ s/ementri/O/g;
        $original =~ s/amenti/O/g;
        $original =~ s/ementi/O/g;

        $original =~ s/uintri/W/g;
        $original =~ s/ontra/W/g;

        $original =~ s/ur/Y/g;
        $original =~ s/uar/Y/g;
        $original =~ s/or/Y/g;

        $original =~ s/^'s/s/;
        $original =~ s/^'n/n/;

        $original =~ s/ins$/1/;
        $original =~ s/in$/1/;
        $original =~ s/ims$/1/;
        $original =~ s/im$/1/;
        $original =~ s/gns$/1/;
        $original =~ s/gn$/1/;

        $original =~ s/mn/5/g;
        $original =~ s/nm/5/g;
        $original =~ s/[mn]/5/g;

        $original =~ s/er/2/g;
        $original =~ s/ar/2/g;

        $original =~ s/b$/3/;
        $original =~ s/p$/3/;
        $original =~ s/v$/4/;
        $original =~ s/f$/4/;

        $primo = $secondo = $original;

        $primo =~ s/'c/A/g;
        $primo =~ s/c[ji]us$/A/;
        $primo =~ s/c[ji]u$/A/;
        $primo =~ s/c'/A/g;
        $primo =~ s/ti/A/g;
        $primo =~ s/ci/A/g;
        $primo =~ s/si/A/g;
        $primo =~ s/zs/A/g;
        $primo =~ s/zi/A/g;
        $primo =~ s/cj/A/g;
        $primo =~ s/çs/A/g;
        $primo =~ s/tz/A/g;
        $primo =~ s/z/A/g;
        $primo =~ s/ç/A/g;
        $primo =~ s/c/A/g;
        $primo =~ s/q/A/g;
        $primo =~ s/k/A/g;
        $primo =~ s/ts/A/g;
        $primo =~ s/s/A/g;

        $secondo =~ s/c$/0/;
        $secondo =~ s/g$/0/;

        $secondo =~ s/bs$/s/;
        $secondo =~ s/cs$/s/;
        $secondo =~ s/fs$/s/;
        $secondo =~ s/gs$/s/;
        $secondo =~ s/ps$/s/;
        $secondo =~ s/vs$/s/;

        $secondo =~ s/di(?=.)/E/g;
        $secondo =~ s/gji/E/g;
        $secondo =~ s/gi/E/g;
        $secondo =~ s/gj/E/g;
        $secondo =~ s/g/E/g;

        $secondo =~ s/ts/E/g;
        $secondo =~ s/s/E/g;
        $secondo =~ s/zi/E/g;
        $secondo =~ s/z/E/g;

        $primo =~ s/j/i/g;
        $secondo =~ s/j/i/g;

        $primo =~ tr/i/i/s;
        $secondo =~ tr/i/i/s;

        $primo =~ s/ai/6/g;
        $primo =~ s/a/6/g;
        $primo =~ s/ei/7/g;
        $primo =~ s/e/7/g;
        $primo =~ s/ou/8/g;
        $primo =~ s/oi/8/g;
        $primo =~ s/o/8/g;
        $primo =~ s/vu/8/g;
        $primo =~ s/u/8/g;
        $primo =~ s/i/7/g;

        $secondo =~ s/ai/6/g;
        $secondo =~ s/a/6/g;
        $secondo =~ s/ei/7/g;
        $secondo =~ s/e/7/g;
        $secondo =~ s/ou/8/g;
        $secondo =~ s/oi/8/g;
        $secondo =~ s/o/8/g;
        $secondo =~ s/vu/8/g;
        $secondo =~ s/u/8/g;
        $secondo =~ s/i/7/g;

        $primo =~ s/^t/H/;
        $primo =~ s/^d/I/;

        $primo =~ s/t/9/g;
        $primo =~ s/d/9/g;

        $secondo =~ s/^t/H/;
        $secondo =~ s/^d/I/;

        $secondo =~ s/t/9/g;
        $secondo =~ s/d/9/g;

        return $primo, $secondo;
    }
    */
}

