using ARLeF.Struments.Apps.CoretorOrtografic.Desktop.ViewModels;
using ARLeF.Struments.Apps.CoretorOrtografic.Desktop.Views;
using ARLeF.Struments.Components.CoretorOrtografic.Core.SpellChecker;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            new MainWindow
            {
                DataContext = new MainWindowViewModel(Locator.GetLocator().GetService<ISpellChecker>())
            }.Show();

            base.OnFrameworkInitializationCompleted();
        }
    }
}
