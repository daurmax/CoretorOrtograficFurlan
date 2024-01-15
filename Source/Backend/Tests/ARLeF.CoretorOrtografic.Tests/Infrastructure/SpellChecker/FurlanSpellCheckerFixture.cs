using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;
using ARLeF.CoretorOrtografic.Core.SpellChecker;
using Autofac;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

                Assert.That(result);
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

                Assert.That(!result);
            }
        }

        [Test]
        public async Task GetWordSuggestions_InvalidWordWithSuggestions_ReturnsSuggestions()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("cjupe");
                var actualSuggestions = await spellChecker.GetWordSuggestions(word);

                Assert.That(actualSuggestions is not null);
                Assert.That(actualSuggestions.Any());

                var expectedSuggestions = new List<string> { "cjape", "cope", "copi", "sope", "supe", "copii", "cjepe", "supi", "zupe", "copiii" };
                Assert.That(actualSuggestions, Is.EqualTo(expectedSuggestions));
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

                Assert.That(suggestions is not null);
                Assert.That(!suggestions.Any());
            }
        }
    }
}
