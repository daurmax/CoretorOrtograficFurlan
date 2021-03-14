using ARLeF.Struments.Base.Core.Development;
using ARLeF.Struments.Base.Core.Output;
using ARLeF.Struments.Base.Infrastructure.Development;
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
        private readonly bool _isDevelopment = false;

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
