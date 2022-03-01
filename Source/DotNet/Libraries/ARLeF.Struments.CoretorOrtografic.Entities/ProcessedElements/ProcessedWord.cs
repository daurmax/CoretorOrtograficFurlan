using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements
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

        public override string ToString()
        {
            return _current;
        }
    }
}
