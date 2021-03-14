using Autofac;
using CoretorOrtografic.Base.Abstractions.Output;
using CoretorOrtografic.Base.Abstractions.Development;
using CoretorOrtografic.Components.Infrastructure.Output;
using CoretorOrtografic.Components.Infrastructure.Development;
using System;
using System.Threading;

namespace CoretorOrtografic.Apps.CLI
{
    class Program
    {
        private static IContainer _container { get; set; }

        static void Main(string[] args)
        {
#if DEBUG
            _container = DependencyContainer.Configure(true);
#else
            _container = DependencyContainer.Configure(false);
#endif
            WriteDate();
        }

        public static void WriteDate()
        {
            // Create the scope, resolve your IDateWriter,
            // use it, then dispose of the scope.
            using (var scope = _container.BeginLifetimeScope())
            {
                var writer = scope.Resolve<IDateWriter>();
                writer.WriteDate();
            }
        }
    }
}
