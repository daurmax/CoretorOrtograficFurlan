using ARLeF.Struments.CoretorOrtografic.Contracts.SpellChecker;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ISpellChecker _spellChecker;

        #region Fields
        private string _text;
        private string _selectedWord;
        private List<string> _suggestedWords;
        private FormattedText _test;
        #endregion Fields

        public MainWindowViewModel(ISpellChecker spellChecker) 
        {
            _spellChecker = spellChecker;

            Text = "prova";
            SpellCheckCommand = ReactiveCommand.Create(ExecuteSpellCheckCommand);
        }

        #region Properties
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
        #endregion Properties

        #region Commands
        public ReactiveCommand<Unit, Unit> SpellCheckCommand { get; }
        private void ExecuteSpellCheckCommand()
        {
            _spellChecker.ExecuteSpellCheck(Text);

            Text = _spellChecker.GetProcessedText();
        }
        #endregion Commands
    }
}
