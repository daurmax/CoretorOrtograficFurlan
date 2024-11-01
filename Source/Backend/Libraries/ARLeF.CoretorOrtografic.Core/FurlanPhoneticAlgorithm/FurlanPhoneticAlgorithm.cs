﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.CoretorOrtografic.Core.Constants;

namespace ARLeF.CoretorOrtografic.Core.FurlanPhoneticAlgorithm
{
	public static class FurlanPhoneticAlgorithm
	{
        public static int Levenshtein(string source, string target)
        {
            int sourceLength = source.Length;
            int targetLength = target.Length;
            int[,] distanceMatrix = new int[sourceLength + 1, targetLength + 1];

            if (sourceLength == 0)
            {
                return targetLength;
            }

            if (targetLength == 0)
            {
                return sourceLength;
            }

            for (int i = 0; i <= sourceLength; i++)
            {
                distanceMatrix[i, 0] = i;
            }

            for (int j = 0; j <= targetLength; j++)
            {
                distanceMatrix[0, j] = j;
            }

            for (int i = 1; i <= sourceLength; i++)
            {
                char sourceChar = source[i - 1];
                for (int j = 1; j <= targetLength; j++)
                {
                    char targetChar = target[j - 1];
                    int cost;

                    if (sourceChar == targetChar)
                    {
                        cost = 0;
                    }
                    else
                    {
                        if (
                            !(FriulianConstants.VOWELS_A.ContainsKey(sourceChar) && FriulianConstants.VOWELS_A.ContainsKey(targetChar)) &&
                            !(FriulianConstants.VOWELS_E.ContainsKey(sourceChar) && FriulianConstants.VOWELS_E.ContainsKey(targetChar)) &&
                            !(FriulianConstants.VOWELS_I.ContainsKey(sourceChar) && FriulianConstants.VOWELS_I.ContainsKey(targetChar)) &&
                            !(FriulianConstants.VOWELS_O.ContainsKey(sourceChar) && FriulianConstants.VOWELS_O.ContainsKey(targetChar)) &&
                            !(FriulianConstants.VOWELS_U.ContainsKey(sourceChar) && FriulianConstants.VOWELS_U.ContainsKey(targetChar))
                           )
                        {
                            cost = 1;
                        }
                        else
                        {
                            cost = 0;
                        }
                    }

                    distanceMatrix[i, j] = Min(distanceMatrix[i - 1, j] + 1, distanceMatrix[i, j - 1] + 1, distanceMatrix[i - 1, j - 1] + cost);
                }
            }

            return distanceMatrix[sourceLength, targetLength];
        }
        public static List<string> SortFriulian(List<string> words)
        {
            return words.OrderBy(word => TranslateWordForSorting(word)).ToList();
        }
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

            secondHash = Regex.Replace(secondHash, "di(?=.)", "E"); // Replaces only if word does not end with "di"
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

            // Remove double "i" in both hashes
            var doubleLetterToRemove = 'i';

            var firstHashResult = new StringBuilder();
            var firstHashToCharArray = firstHash.ToCharArray();
            for (int i = 0; i < firstHashToCharArray.Length - 1; i++)
            {
                if (firstHashToCharArray[i] == doubleLetterToRemove && firstHashToCharArray[i] == firstHashToCharArray[i + 1])
                {
                    continue;
                }

                if (i == firstHashToCharArray.Length - 2)
                {
                    firstHashResult.Append(firstHashToCharArray[i]);
                    firstHashResult.Append(firstHashToCharArray[firstHashToCharArray.Length - 1]);
                }
                else
                {
                    firstHashResult.Append(firstHashToCharArray[i]);
                }
            }
            firstHash = firstHashResult.ToString();

            var secondHashResult = new StringBuilder();
            var secondHashToCharArray = secondHash.ToCharArray();
            for (int i = 0; i < secondHashToCharArray.Length - 1; i++)
            {
                if (secondHashToCharArray[i] == doubleLetterToRemove && secondHashToCharArray[i] == secondHashToCharArray[i + 1])
                {
                    continue;
                }

                if (i == secondHashToCharArray.Length - 2)
                {
                    secondHashResult.Append(secondHashToCharArray[i]);
                    secondHashResult.Append(secondHashToCharArray[secondHashToCharArray.Length - 1]);
                }
                else
                {
                    secondHashResult.Append(secondHashToCharArray[i]);
                }
            }
            secondHash = secondHashResult.ToString();

            firstHash = Regex.Replace(firstHash, "ai", "6");
            firstHash = Regex.Replace(firstHash, "a", "6");
            firstHash = Regex.Replace(firstHash, "ei", "7");
            firstHash = Regex.Replace(firstHash, "e", "7");
            firstHash = Regex.Replace(firstHash, "ou", "8");
            firstHash = Regex.Replace(firstHash, "oi", "8");
            firstHash = Regex.Replace(firstHash, "o", "8");
            firstHash = Regex.Replace(firstHash, "vu", "8");
            firstHash = Regex.Replace(firstHash, "u", "8");
            firstHash = Regex.Replace(firstHash, "i", "7");

            secondHash = Regex.Replace(secondHash, "ai", "6");
            secondHash = Regex.Replace(secondHash, "a", "6");
            secondHash = Regex.Replace(secondHash, "ei", "7");
            secondHash = Regex.Replace(secondHash, "e", "7");
            secondHash = Regex.Replace(secondHash, "ou", "8");
            secondHash = Regex.Replace(secondHash, "oi", "8");
            secondHash = Regex.Replace(secondHash, "o", "8");
            secondHash = Regex.Replace(secondHash, "vu", "8");
            secondHash = Regex.Replace(secondHash, "u", "8");
            secondHash = Regex.Replace(secondHash, "i", "7");

            firstHash = Regex.Replace(firstHash, "^t", "H"); // Only at the beginning
            firstHash = Regex.Replace(firstHash, "^d", "I"); // Only at the beginning

            firstHash = Regex.Replace(firstHash, "t", "9");
            firstHash = Regex.Replace(firstHash, "d", "9");

            secondHash = Regex.Replace(secondHash, "^t", "H"); // Only at the beginning
            secondHash = Regex.Replace(secondHash, "^d", "I"); // Only at the beginning

            secondHash = Regex.Replace(secondHash, "t", "9");
            secondHash = Regex.Replace(secondHash, "d", "9");

            return (firstHash, secondHash);
        }
        private static string TranslateWordForSorting(string word)
        {
            string translatedWord = string.Empty;
            string originalChars = "0123456789âäàáÄÁÂÀAaBCçÇDéêëèÉÊËÈEeFGHïîìíÍÎÏÌIiJKLMNôöòóÓÔÒÖOoPQRSTÚÙÛÜúûùüuUVWXYZ";
            string sortedChars = "\x30\x31\x32\x33\x34\x35\x36\x37\x38\x39aaaaaaaaaabcccdeeeeeeeeeefghiiiiiiiiiijklmnoooooooooopqrstuuuuuuuuuuvwxyz";

            foreach (char c in word)
            {
                int index = originalChars.IndexOf(c);
                if (index >= 0)
                {
                    translatedWord += sortedChars[index];
                }
                else
                {
                    translatedWord += c;
                }
            }

            return translatedWord.Replace("^'s", "s");
        }
        private static int Min(int a, int b, int c)
        {
            return Math.Min(Math.Min(a, b), c);
        }
    }
}