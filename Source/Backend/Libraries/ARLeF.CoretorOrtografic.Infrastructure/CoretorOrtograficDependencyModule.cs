using ARLeF.CoretorOrtografic.Infrastructure.ContentReader;
using ARLeF.CoretorOrtografic.Infrastructure.KeyValueDatabase;
using ARLeF.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.CoretorOrtografic.Core.Input;
using ARLeF.CoretorOrtografic.Core.KeyValueDatabase;
using ARLeF.CoretorOrtografic.Core.SpellChecker;
using Autofac;

namespace ARLeF.CoretorOrtografic.Business
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
                case CallerApplicationEnum.Web:
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
            builder.RegisterType<FurlanSpellChecker>().As<ISpellChecker>();
            builder.RegisterType<SQLiteKeyValueDatabase>().As<IKeyValueDatabase>();
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
