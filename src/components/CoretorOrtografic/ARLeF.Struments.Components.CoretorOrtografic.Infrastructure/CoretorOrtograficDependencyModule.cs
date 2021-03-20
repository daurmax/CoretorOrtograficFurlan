using ARLeF.Struments.Base.Core.Input;
using ARLeF.Struments.Base.Core.Output;
using ARLeF.Struments.Base.Infrastructure;
using ARLeF.Struments.Components.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.Components.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.ContentReader;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.KeyValueDatabase;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.Output;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.SpellChecker;
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
        private readonly bool _isDevelopment = false;
        private CallerApplicationEnum _callerApplication;

        public CoretorOrtograficDependencyModule(bool isDevelopment, CallerApplicationEnum callerApplication)
        {
            _isDevelopment = isDevelopment;
            _callerApplication = callerApplication;
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

            switch (_callerApplication)
            {
                case CallerApplicationEnum.CLI:
                    RegisterCLIDependencies(builder);
                    break;
                case CallerApplicationEnum.Desktop:
                    RegisterDesktopDependencies(builder);
                    break;
                case CallerApplicationEnum.Mobile:
                    RegisterMobileDependencies(builder);
                    break;
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
            builder.RegisterType<MockSpellChecker>().As<ISpellChecker>();
            builder.RegisterType<MockKeyValueDatabase>().As<IKeyValueDatabase>();
        }
        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // Add production only services
        }

        private void RegisterCLIDependencies(ContainerBuilder builder)
        {
            // Add CLI only services
            builder.RegisterType<ConsoleContentReader>().As<IContentReader>();
            builder.RegisterType<ConsoleContentWriter>().As<IContentWriter>();
        }
        private void RegisterDesktopDependencies(ContainerBuilder builder)
        {
            // Add desktop only services
        }
        private void RegisterMobileDependencies(ContainerBuilder builder)
        {
            // Add mobile only services
        }
    }
}
