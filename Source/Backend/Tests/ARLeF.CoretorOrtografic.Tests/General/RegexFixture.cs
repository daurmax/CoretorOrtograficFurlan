using ARLeF.CoretorOrtografic.Core.Constants;
using Autofac;
using NUnit.Framework;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ARLeF.CoretorOrtografic.Tests.General
{
    public class RegexFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup() { }

        [Test]
        public void ReplaceUncommonApostrophesTest()
        {
            string input = "‘’‘’";
            string expected = "''''";
            Assert.That(!input.Equals(expected));

            string result = Regex.Replace(input, FriulianConstants.UNCOMMON_APOSTROPHES, "'");

            Assert.That(expected.Equals(result));
        }

        [Test]
        public void ReplaceLetterEWithApostrophTest()
        {
            string input = "e poi";
            string expected = "'poi";

            string result = Regex.Replace(input, "e ", "'");

            Assert.That(expected.Equals(result));
        }

        [Test]
        public void RemoveAllWhiteSpacesTest()
        {
            string input = "remove    all whitespaces  from             this string";
            string expected = "removeallwhitespacesfromthisstring";

            string result = Regex.Replace(input, " ", "");

            Assert.That(expected.Equals(result));
        }

        [Test]
        public void RemoveAllRepeatingLettersTest()
        {
            var doubles = "suppellettile";
            var triples = "supppellettile";
            var expectedResult = "supeletile";
            var doublesStrResult = new StringBuilder();
            var triplesStrResult = new StringBuilder();

            foreach (var element in doubles.ToCharArray())
            {
                if (doublesStrResult.Length == 0 || doublesStrResult[doublesStrResult.Length - 1] != element)
                    doublesStrResult.Append(element);
            }
            var doubleResult = doublesStrResult.ToString();

            foreach (var element in triples.ToCharArray())
            {
                if (triplesStrResult.Length == 0 || triplesStrResult[triplesStrResult.Length - 1] != element)
                    triplesStrResult.Append(element);
            }
            var tripleResult = triplesStrResult.ToString();

            Assert.That(doubleResult.Equals(expectedResult));
            Assert.That(tripleResult.Equals(expectedResult));
        }

        [Test]
        public void RemoveSpecificRepeatingLettersTest()
        {
            var doubles = "suppellettile";
            var doublesToCharArray = doubles.ToCharArray();
            var triples = "supppellettile";
            var triplesToCharArray = triples.ToCharArray();
            var expectedResult = "supellettile";
            var letterToRemove = 'p';
            var doublesStrResult = new StringBuilder();
            var triplesStrResult = new StringBuilder();

            for (int i = 0; i < doublesToCharArray.Length - 1; i++)
            {
                if (doublesToCharArray[i] == letterToRemove && doublesToCharArray[i] == doublesToCharArray[i + 1])
                {
                    continue;
                }

                if (i == doublesToCharArray.Length - 2)
                {
                    doublesStrResult.Append(doublesToCharArray[i]);
                    doublesStrResult.Append(doublesToCharArray[doublesToCharArray.Length - 1]);
                }
                else
                {
                    doublesStrResult.Append(doublesToCharArray[i]);
                }
            }
            var doubleResult = doublesStrResult.ToString();

            for (int i = 0; i < triplesToCharArray.Length - 1; i++)
            {
                if (triplesToCharArray[i] == letterToRemove && triplesToCharArray[i] == triplesToCharArray[i + 1])
                {
                    continue;
                }

                if (i == triplesToCharArray.Length - 2)
                {
                    triplesStrResult.Append(triplesToCharArray[i]);
                    triplesStrResult.Append(triplesToCharArray[triplesToCharArray.Length - 1]);
                }
                else
                {
                    triplesStrResult.Append(triplesToCharArray[i]);
                }
            }
            var tripleResult = triplesStrResult.ToString();

            Assert.That(doubleResult.Equals(expectedResult));
            Assert.That(tripleResult.Equals(expectedResult));
        }

        [Test]
        public void ReplaceFriulianVowelsWithLatinVowelsTest()
        {
            string input = "àáâ'a èéê'e ìíî'i òóô'o ùúû'u";
            string expected = "aaaa eeee iiii oooo uuuu";
            string result = String.Empty;

            result = Regex.Replace(input, FriulianConstants.SMALL_A_VARIANTS, "a");
            result = Regex.Replace(result, FriulianConstants.SMALL_E_VARIANTS, "e");
            result = Regex.Replace(result, FriulianConstants.SMALL_I_VARIANTS, "i");
            result = Regex.Replace(result, FriulianConstants.SMALL_O_VARIANTS, "o");
            result = Regex.Replace(result, FriulianConstants.SMALL_U_VARIANTS, "u");

            Assert.That(expected.Equals(result));
        }
    }
}
