using System;
using System.Diagnostics;
using ARLeF.CoretorOrtografic.Dictionaries.Constants;
using ARLeF.CoretorOrtografic.Core.RadixTree;
using NUnit.Framework;
using Autofac.Core;
using Autofac;
using ARLeF.CoretorOrtografic.Core.Input;
using ARLeF.CoretorOrtografic.Core.SpellChecker;
using ARLeF.CoretorOrtografic.Core.KeyValueDatabase;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ARLeF.CoretorOrtografic.Core.Enums;
using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;
using System.Threading.Tasks;
using ARLeF.CoretorOrtografic.Infrastructure.SpellChecker;

namespace ARLeF.CoretorOrtografic.Tests.Infrastructure.SpellChecker
{
    public class FurlanSpellCheckerFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = CoretorOrtograficTestDependencyContainer.Configure(true);
        }

        [Test]
        public async Task CheckWord_CorrectWord_ReturnsTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("cjape");
                bool result = await spellChecker.CheckWord(word);

                Assert.IsTrue(result);
            }
        }

        [Test]
        public async Task CheckWord_IncorrectWord_ReturnsFalse()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("incorrectword");
                bool result = await spellChecker.CheckWord(word);

                Assert.IsFalse(result);
            }
        }

        [Test]
        public async Task GetWordSuggestions_InvalidWordWithSuggestions_ReturnsSuggestions()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("cjupe");
                var suggestions = await spellChecker.GetWordSuggestions(word);

                Assert.IsNotNull(suggestions);
                Assert.IsNotEmpty(suggestions);


                var expectedSuggestions = new List<string> { "cjape", "cope", "copi", "sope", "supe", "copii", "cjepe", "supi", "zupe", "copiii" };
                Assert.AreEqual(expectedSuggestions, suggestions);
            }
        }

        [Test]
        public async Task GetWordSuggestions_InvalidWordNoSuggestions_ReturnsEmptySuggestions()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("invalidwordnosuggestions");
                var suggestions = await spellChecker.GetWordSuggestions(word);

                Assert.IsNotNull(suggestions);
                Assert.IsEmpty(suggestions);
            }
        }
    }
}
