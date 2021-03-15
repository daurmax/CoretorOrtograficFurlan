using ARLeF.Struments.Base.Core.Development;
using ARLeF.Struments.Base.Core.Input;
using ARLeF.Struments.Base.Core.Output;
using ARLeF.Struments.Components.CoretorOrtografic.Core.SpellChecker;
using ARLeF.Struments.Components.CoretorOrtografic.Infrastructure.SpellChecker;
using Autofac;
using System;
using System.Threading;

namespace ARLeF.Struments.Apps.CoretorOrtografic.CLI
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

                MockSpellChecker checker = new MockSpellChecker();
                checker.ExecuteSpellCheck(reader.Read());
                writer.Write(String.Join(" ", checker.AllIncorrectWordList));
            }
        }
    }
}
