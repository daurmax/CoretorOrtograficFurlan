using ARLeF.Struments.Base.Infrastructure;
using ARLeF.Struments.Components.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.KeyValueDatabase;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.Components.CoretorOrtografic.Infrastructure
{
    public class CoretorOrtograficDependencyModule : Module
    {
        private bool _isDevelopment = false;

        public CoretorOrtograficDependencyModule(bool isDevelopment)
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
            builder.RegisterModule(new CoreDependencyModule(_isDevelopment));
        }
        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // Add development only services
            builder.RegisterType<MockKeyValueDatabase>().As<IKeyValueDatabase>();
        }
        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // Add production only services
        }
    }
}
