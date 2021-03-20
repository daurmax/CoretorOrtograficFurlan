using ARLeF.Struments.Apps.CoretorOrtografic.Desktop.Extensions;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;
using Splat.Autofac;
using System;
using System.Reflection;
using System.Threading;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            // Build a new Autofac container with specific business logic.
            var builder = CoretorOrtograficDesktopDependencyContainer.Configure
#if DEBUG
                (true);
#else
                (false);
#endif
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            // Use Autofac for ReactiveUI dependency resolution.
            // After we call the method below, Locator.Current and
            // Locator.CurrentMutable start using Autofac locator.
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(builder);
            Locator.SetLocator(resolver);

            // These .InitializeX() methods will add ReactiveUI platform 
            // registrations to your container. They MUST be present if
            // you *override* the default Locator.
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();
            Locator.CurrentMutable.InitializeAvalonia();

            resolver.SetLifetimeScope(builder.Build());
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
