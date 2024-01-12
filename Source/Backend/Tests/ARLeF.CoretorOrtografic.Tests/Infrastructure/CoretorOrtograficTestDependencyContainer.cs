using ARLeF.CoretorOrtografic.Business;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.CoretorOrtografic.Tests.Infrastructure
{
    class CoretorOrtograficTestDependencyContainer
    {
        public static IContainer Configure(bool isDevelopment)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoretorOrtograficDependencyModule(isDevelopment, CallerApplicationEnum.CLI));

            return builder.Build();
        }
    }
}
