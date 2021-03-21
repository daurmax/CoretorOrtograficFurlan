using ARLeF.Struments.CoretorOrtografic.Business.ContentReader;
using ARLeF.Struments.CoretorOrtografic.Business.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Business.Output;
using ARLeF.Struments.CoretorOrtografic.Business.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Contracts.Input;
using ARLeF.Struments.CoretorOrtografic.Contracts.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Contracts.Output;
using ARLeF.Struments.CoretorOrtografic.Contracts.SpellChecker;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARLeF.Struments.CoretorOrtografic.Business
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
