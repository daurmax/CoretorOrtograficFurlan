using ARLeF.CoretorOrtografic.Core.Enums;
using ARLeF.CoretorOrtografic.Core.Extensions;
using System.Collections.Generic;

namespace ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements
{
    public class ProcessedWord : IProcessedElement
    {
        private readonly string _original;
        private string _current;
        private bool _checked;
        private bool _correct;

        public ProcessedWord(string word)
        {
            _original = _current = word;
        }

        public string Original { get => _original; }
        public string Current
        {
            get => _current;
            set
            {
                _current = value;
            }
        }
        public bool Checked
        {
            get => _checked;
            set => _checked = value;
        }
        public bool Correct
        {
            get => _correct;
            set => _correct = value;
        }

        public WordType Case
        {
            get
            {
                string word = _original;
                string lcWord = _original.ToLower();
                string ucWord = _original.ToUpper();
                string ucfWord = _original.ToFirstCharacterUpper();

                if (word == lcWord) return WordType.Lowercase;
                if (word == ucfWord) return WordType.FirstLetterUppercase;
                if (word == ucWord) return WordType.Uppercase;

                return default;
            }
        }

        public List<string> Suggestions { get; set; } = new List<string>();

        public override string ToString()
        {
            return _current;
        }
    }
}
