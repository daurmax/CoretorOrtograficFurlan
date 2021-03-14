using ARLeF.Struments.Base.Core.Development;
using ARLeF.Struments.Base.Core.Output;
using ARLeF.Struments.Base.Infrastructure.Development;
using ARLeF.Struments.Base.Infrastructure.Output;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Base.Infrastructure
{
    public class CoreDependencyModule : Module
    {
        private bool _isDevelopment = false;

        public CoreDependencyModule(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }
            RegisterCommonDependencies(builder);
        }

        private void RegisterCommonDependencies(ContainerBuilder builder)
        {
            // Add services for both production and development
            builder.RegisterType<ConsoleOutput>().As<IOutput>();
        }
        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // Add development only services
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
        }
        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // Add production only services
        }
    }
}
