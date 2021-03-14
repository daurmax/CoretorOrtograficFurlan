using Autofac;
using CoretorOrtografic.Base.Abstractions.Output;
using CoretorOrtografic.Base.Abstractions.Development;
using CoretorOrtografic.Components.Infrastructure.Output;
using CoretorOrtografic.Components.Infrastructure.Development;
using System;
using System.Threading;

namespace CoretorOrtografic.CLI
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleOutput>().As<IOutput>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            Container = builder.Build();

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
