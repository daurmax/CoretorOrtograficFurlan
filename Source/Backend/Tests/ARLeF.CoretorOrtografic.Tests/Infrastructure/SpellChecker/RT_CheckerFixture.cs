using ARLeF.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.CoretorOrtografic.Infrastructure.SpellChecker;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ARLeF.CoretorOrtografic.Tests.Infrastructure.SpellChecker
{
    public class RT_CheckerFixture
    {
        private ARLeF.CoretorOrtografic.Core.RadixTree.RadixTree _rt;
        private RT_Checker _checker;

        [SetUp]
        public void Setup()
        {
            _rt = new ARLeF.CoretorOrtografic.Core.RadixTree.RadixTree(DictionaryFilePaths.WORDS_RADIX_TREE_FILE_PATH);
            _checker = new RT_Checker(_rt);
        }

        [Test]
        public void HasWord_FriulianWordShouldExist()
        {
            var word = "cjape";
            bool actual = _checker.HasWord(word);
            Assert.That(actual);
        }

        [Test]
        public void HasWord_NotFriulianWordShouldNotExist()
        {
            var word = "orange";
            bool actual = _checker.HasWord(word);
            Assert.That(!actual);
        }

        [Test]
        public void GetWordsEd1_CorrectSuggestionsFor_cjupe_()
        {
            // Redirect console output to a text file
            string outputFile = "GetWordsEd1_CorrectSuggestionsForCjupe.txt";
            List<string> expectedSuggestions = new();
            List<string> actualSuggestions = new();
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                Console.SetOut(writer);

                var word = "cjupe";
                expectedSuggestions = new List<string> { "cjape", "cjepe", "cjope", "clupe", "crupe" };
                actualSuggestions = _checker.GetWordsED1(word).ToList();

                writer.WriteLine("Word: " + word);
                writer.WriteLine("Expected suggestions: " + string.Join(", ", expectedSuggestions));
                writer.WriteLine("Actual suggestions: " + string.Join(", ", actualSuggestions));
            }

            // Reset console output to its original output stream
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);

            Assert.That(actualSuggestions, Is.EqualTo(expectedSuggestions));
        }

        [Test]
        public void GetWordsEd1_CorrectSuggestionsFor_tuint_()
        {
            // Redirect console output to a text file
            string outputFile = "GetWordsEd1_CorrectSuggestionsForTuint.txt";
            List<string> expectedSuggestions = new();
            List<string> actualSuggestions = new();
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                Console.SetOut(writer);

                var word = "tuint";
                expectedSuggestions = new List<string> { "atuint", "buint", "cuint", "fuint", "guint", "luint*", "muint", "puint", "quint*", "suint", "stuint", "taint", "tint", "tudint", "tufint", "tuins", "tuinut", "tuin", "zuint" };
                actualSuggestions = _checker.GetWordsED1(word).ToList();

                writer.WriteLine("Word: " + word);
                writer.WriteLine("Expected suggestions: " + string.Join(", ", expectedSuggestions));
                writer.WriteLine("Actual suggestions: " + string.Join(", ", actualSuggestions));
            }

            // Reset console output to its original output stream
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);

            Assert.That(actualSuggestions, Is.EqualTo(expectedSuggestions));
        }

        [Test]
        public void GetWordsEd1_CorrectSuggestionsFor_purfit_()
        {
            // Redirect console output to a text file
            string outputFile = "GetWordsEd1_CorrectSuggestionsForPurfit.txt";
            List<string> expectedSuggestions = new();
            List<string> actualSuggestions = new();
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                Console.SetOut(writer);

                var word = "purfit";
                expectedSuggestions = new List<string> { "perfit", "purcit" };
                actualSuggestions = _checker.GetWordsED1(word).ToList();

                writer.WriteLine("Word: " + word);
                writer.WriteLine("Expected suggestions: " + string.Join(", ", expectedSuggestions));
                writer.WriteLine("Actual suggestions: " + string.Join(", ", actualSuggestions));
            }

            // Reset console output to its original output stream
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);

            Assert.That(actualSuggestions, Is.EqualTo(expectedSuggestions));
        }

        [Test]
        public void GetWordsEd1_NoSuggestionsForInvalidWord()
        {
            var word = "invalidwordnosuggestions";
            var actualSuggestions = _checker.GetWordsED1(word);
            Assert.That(!actualSuggestions.Any());
        }
    }
}
