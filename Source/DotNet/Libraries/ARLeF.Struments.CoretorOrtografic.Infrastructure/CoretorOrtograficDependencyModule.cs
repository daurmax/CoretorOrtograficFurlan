using ARLeF.Struments.CoretorOrtografic.Infrastructure.ContentReader;
using ARLeF.Struments.CoretorOrtografic.Infrastructure.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using Autofac;
using ARLeF.Struments.CoretorOrtografic.Core.RadixTreeReader;

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
            builder.RegisterType<FurlanSpellChecker>().As<ISpellChecker>();
            builder.RegisterType<SQLiteKeyValueDatabase>().As<IKeyValueDatabase>();
        }
        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // Add production only services
        }

        private void RegisterCLIDependencies(ContainerBuilder builder)
        {
            // Add CLI only services
            builder.RegisterType<ConsoleContentReader>().As<IContentReader>();
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
