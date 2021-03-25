using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop.Views
{
    public class MainWindow : Window
    {
        internal TextEditor textEditor;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            textEditor = this.FindControl<TextEditor>("textEditor");
        }
    }
}
