using ARLeF.Struments.Base.Core.Development;
using Autofac;
using System;
using System.Threading;

namespace ARLeF.Struments.Apps.CoretorOrtografic.CLI
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
