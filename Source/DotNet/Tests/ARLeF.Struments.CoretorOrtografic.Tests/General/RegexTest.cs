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

namespace ARLeF.Struments.CoretorOrtografic.Tests.General
{
    public class RegexTest
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup() { }

        [Test]
        public void ReplaceUncommonApostrophes()
        {
            string input = "‘’‘’";
            string expected = "''''";
            Assert.AreNotEqual(input, expected);

            string result = Regex.Replace(input, FriulianConstants.UNCOMMON_APOSTROPHES, "'");

            Assert.AreEqual(expected, result);
        }
    }
}
