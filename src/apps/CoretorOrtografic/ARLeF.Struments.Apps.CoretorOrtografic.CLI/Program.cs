using ARLeF.Struments.Base.Core.Development;
using Autofac;
using System;
using System.Threading;

namespace ARLeF.Struments.Apps.CoretorOrtografic.CLI
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
#if DEBUG
            Container = CoretorOrtograficCliDependencyContainer.Configure(true);
#else
            _container = CoretorOrtograficCliDependencyContainer.Configure(false);
#endif
            WriteDate();
        }

        public static void WriteDate()
        {
            // Create the scope, resolve your IDateWriter,
            // use it, then dispose of the scope.
            using (var scope = Container.BeginLifetimeScope())
            {
                var writer = scope.Resolve<IDateWriter>();
                writer.WriteDate();
            }
        }
    }
}
