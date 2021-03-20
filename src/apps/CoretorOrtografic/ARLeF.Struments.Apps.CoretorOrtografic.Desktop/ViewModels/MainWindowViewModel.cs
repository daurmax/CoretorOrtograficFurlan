using ARLeF.Struments.CoretorOrtografic.Contracts.SpellChecker;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ISpellChecker _spellChecker;

        public MainWindowViewModel(ISpellChecker spellChecker) 
        {
            _spellChecker = spellChecker;
        }
    }
}
