using ARLeF.Struments.Base.Core.Development;
using ARLeF.Struments.Base.Core.Output;
using ARLeF.Struments.Base.Infrastructure.Development;
using ARLeF.Struments.Base.Infrastructure.Output;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Apps.CoretorOrtografic.CLI
{
    class DependencyContainer
    {
        public static IContainer Configure(bool isDevelopment)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoretorOrtograficDependencyModule(isDevelopment));

            return builder.Build();
        }
    }
}
