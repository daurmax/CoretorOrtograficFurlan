using ARLeF.Struments.CoretorOrtografic.Contracts.SpellChecker;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ISpellChecker _spellChecker;

        private string _text;
        private string _selectedWord;
        private List<string> _suggestedWords;

        public MainWindowViewModel(ISpellChecker spellChecker) 
        {
            _spellChecker = spellChecker;
        }

        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }
        public string SelectedWord
        {
            get => _selectedWord;
            set => this.RaiseAndSetIfChanged(ref _selectedWord, value);
        }
        public List<string> SuggestedWords
        {
            get => _suggestedWords;
            set => this.RaiseAndSetIfChanged(ref _suggestedWords, value);
        }
    }
}
