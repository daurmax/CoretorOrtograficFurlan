﻿using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Apps.CoretorOrtografic.Desktop
{
    class CoretorOrtograficDesktopDependencyContainer
    {
        public static ContainerBuilder Configure(bool isDevelopment)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoretorOrtograficDependencyModule(isDevelopment, CallerApplicationEnum.Desktop));

            return builder;
        }
    }
}