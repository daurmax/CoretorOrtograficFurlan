using Autofac;
using CoretorOrtografic.Base.Abstractions.Development;
using CoretorOrtografic.Base.Abstractions.Output;
using CoretorOrtografic.Components.Infrastructure.Development;
using CoretorOrtografic.Components.Infrastructure.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoretorOrtografic.Apps.CLI
{
    class DependencyContainer
    {
        public static IContainer Configure(bool isDevelopment)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleOutput>().As<IOutput>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            return builder.Build();
        }
    }
}
