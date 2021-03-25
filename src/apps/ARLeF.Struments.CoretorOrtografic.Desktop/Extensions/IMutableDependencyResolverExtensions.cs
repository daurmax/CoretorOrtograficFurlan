using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Desktop.Extensions
{
    public static class IMutableDependencyResolverExtensions
    {
        public static void InitializeAvalonia(this IMutableDependencyResolver resolver)
        {
            resolver.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
            resolver.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        }
    }
}
