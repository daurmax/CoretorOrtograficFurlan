using ARLeF.Struments.CoretorOrtografic.DesktopAvalonia.ViewModels;
using ARLeF.Struments.CoretorOrtografic.DesktopAvalonia.Views;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;

namespace ARLeF.Struments.CoretorOrtografic.DesktopAvalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(Locator.GetLocator().GetService<ISpellChecker>()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
