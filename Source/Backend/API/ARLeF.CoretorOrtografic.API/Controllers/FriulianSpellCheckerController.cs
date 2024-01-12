using ARLeF.CoretorOrtografic.Core.SpellChecker;
using Microsoft.AspNetCore.Mvc;

namespace ARLeF.CoretorOrtografic.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FriulianSpellCheckerController : ControllerBase
    {
        private readonly ISpellChecker _spellChecker;
        private readonly ILogger<FriulianSpellCheckerController> _logger;

        public FriulianSpellCheckerController(ISpellChecker spellChecker, ILogger<SpellCheckerController> logger)
        {
            _spellChecker = spellChecker;
            _logger = logger;
        }

        [HttpGet("check/{word}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CheckWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return BadRequest("Word is required.");

            _spellChecker.ExecuteSpellCheck(word);
            var processedWord = _spellChecker.ProcessedWords.FirstOrDefault();

            if (processedWord == null)
                return BadRequest("Error processing the word.");

            return Ok(new { Word = processedWord.Original, IsCorrect = processedWord.Correct });
        }

        [HttpGet("suggest/{word}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SuggestWords(string word)
        {
            if (string.IsNullOrEmpty(word))
                return BadRequest("Word is required.");

            _spellChecker.ExecuteSpellCheck(word);
            var processedWord = _spellChecker.ProcessedWords.FirstOrDefault();

            if (processedWord == null)
                return BadRequest("Error processing the word.");

            var suggestions = await _spellChecker.GetWordSuggestions(processedWord);

            return Ok(new { Word = processedWord.Original, Suggestions = suggestions });
        }
    }
}
