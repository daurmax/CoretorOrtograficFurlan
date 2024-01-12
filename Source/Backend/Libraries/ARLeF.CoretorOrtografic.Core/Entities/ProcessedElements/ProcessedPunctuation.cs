using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements
{
    public class ProcessedPunctuation : IProcessedElement
    {
        private readonly string _original;
        private string _current;

        public ProcessedPunctuation(string punctiation)
        {
            _original = _current = punctiation;
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

        public override string ToString()
        {
            return _current;
        }
    }
}
