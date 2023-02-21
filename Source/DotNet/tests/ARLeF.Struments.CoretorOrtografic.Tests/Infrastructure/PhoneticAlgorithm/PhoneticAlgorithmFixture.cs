using System;
using System.Diagnostics;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.FurlanPhoneticAlgorithm;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.PhoneticAlgorithm
{
    public class PhoneticAlgorithmFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup() { }

        [Test]
        public void CommonFriulianWordsPhoneticHashesTest()
        {
            string word = "cjatâ";
            (string, string) expectedHashes = ("A696", "c7696");
            (string, string) calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "'savote";
            expectedHashes = ("A6v897", "E6v897");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "çavatis";
            expectedHashes = ("A6v6AA", "ç6v697E");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "diretamentri";
            expectedHashes = ("I7r79O", "Er79O");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "sdrumâ";
            expectedHashes = ("A9r856", "E9r856");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "rinfuarçadis";
            expectedHashes = ("r75fYA697A", "r75fYç6EE");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "marilenghe";
            expectedHashes = ("527X7", "527X7");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "mandi";
            expectedHashes = ("56597", "56597");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);

            word = "dindi";
            expectedHashes = ("I7597", "E597");
            calculatedHashes = FurlanPhoneticAlgorithm.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);
        }
    }
}
