using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader;
using ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm;
using System.Text.RegularExpressions;
using ARLeF.Struments.CoretorOrtografic.Core.Constants;
using System.Text;

namespace ARLeF.Struments.CoretorOrtografic.Tests.General
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
            Assert.AreNotEqual(input, expected);

            string result = Regex.Replace(input, FriulianConstants.UNCOMMON_APOSTROPHES, "'");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ReplaceLetterEWithApostrophTest()
        {
            string input = "e poi";
            string expected = "'poi";

            string result = Regex.Replace(input, "e ", "'");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveAllWhiteSpacesTest()
        {
            string input = "remove    all whitespaces  from             this string";
            string expected = "removeallwhitespacesfromthisstring";

            string result = Regex.Replace(input, " ", "");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveDoubleLettersTest()
        {
            var doubles = "doppia";
            var triples = "dopppia";
            var expectedResult = "dopia";
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

            Assert.AreEqual(doubleResult, expectedResult);
            Assert.AreEqual(tripleResult, expectedResult);
        }
    }
}
