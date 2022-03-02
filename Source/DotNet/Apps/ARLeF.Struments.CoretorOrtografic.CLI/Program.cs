using ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.Output;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using Autofac;
using System;
using System.Threading;

namespace ARLeF.Struments.CoretorOrtografic.CLI
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            Container = CoretorOrtograficCliDependencyContainer.Configure
#if DEBUG
                (true);
#else
                (false);
#endif

            using (var scope = Container.BeginLifetimeScope())
            {
                var reader = scope.Resolve<IContentReader>();
                var writer = scope.Resolve<IContentWriter>();

                ISpellChecker checker = scope.Resolve<ISpellChecker>();
                checker.ExecuteSpellCheck(reader.Read());
                writer.Write(String.Join("\n", checker.GetAllIncorrectWords()));
            }
        }
    }
}
