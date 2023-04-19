using ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTree;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARLeF.Struments.CoretorOrtografic.Dictionaries.Constants;

namespace ARLeF.Struments.CoretorOrtografic.Tests.Infrastructure.SpellChecker
{
    public class RT_CheckerFixture
    {
        private Core.RadixTree.RadixTree _rt;
        private RT_Checker _checker;

        [SetUp]
        public void Setup()
        {
            _rt = new Core.RadixTree.RadixTree(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH);
            _checker = new RT_Checker(_rt);
        }

        [Test]
        public void HasWord_FriulianWordShouldExist()
        {
            var word = "cjape";
            bool actual = _checker.HasWord(word);
            Assert.IsTrue(actual);
        }

        [Test]
        public void HasWord_NotFriulianWordShouldNotExist()
        {
            var word = "orange";
            bool actual = _checker.HasWord(word);
            Assert.IsFalse(actual);
        }

        [Test]
        public void GetWordsEd1_CorrectSuggestionsForCjupe()
        {
            var word = "cjupe";
            var expectedSuggestions = new List<string> { "cjape", "cjepe", "cjope", "clupe", "crupe" };
            var actualSuggestions = _checker.GetWordsED1(word);
            CollectionAssert.AreEquivalent(expectedSuggestions, actualSuggestions);
        }

        [Test]
        public void GetWordsEd1_NoSuggestionsForInvalidWord()
        {
            var word = "invalidwordnosuggestions";
            var actualSuggestions = _checker.GetWordsED1(word);
            Assert.IsEmpty(actualSuggestions);
        }
    }
}
