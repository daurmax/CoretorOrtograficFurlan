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

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.RadixTree
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
            (string, string) calculatedHashes = FurlanPhoneticAlgorithmModel.GetPhoneticHashesByWord(word);

            Assert.AreEqual(expectedHashes.Item1, calculatedHashes.Item1);
            Assert.AreEqual(expectedHashes.Item2, calculatedHashes.Item2);
        }

        //[Test]
        //public void PrepareOriginalWordTest()
        //{
        //    string word = "verarmentâsicuintri";
        //    string expectedResult = "v225e5tasicW";
        //    string result = FurlanPhoneticAlgorithmModel.PrepareOriginalWord(word);

        //    Assert.AreEqual(expectedResult, result);
        //}
    }
}
