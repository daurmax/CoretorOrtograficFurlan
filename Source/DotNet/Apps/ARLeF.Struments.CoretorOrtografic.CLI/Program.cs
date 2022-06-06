using ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.Output;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using Autofac;
using System;
using System.Threading;
using System.Linq;
using ARLeF.Struments.Apps.CoretorOrtografic.CLI.Enums;

namespace ARLeF.Struments.CoretorOrtografic.CLI
{
    class Program
    {
        private static IContainer Container { get; set; }

        public static void Main(string[] args)
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
                while (true)
                {
                    var readStrings = reader.Read()?.Split(" ").ToList();

                    if (readStrings is null || !readStrings.Any())
                    {
                        writer.Write("No commands or words were provided.");
                        PrintInstructions();
                    }
                    else if (readStrings.Count == 1)
                    {
                        writer.Write("Please provide a word to check.");
                        PrintInstructions();
                    }
                    else if (readStrings.First().Length != 1)
                    {
                        writer.Write($"Unknown command '{readStrings.First()}'.");
                        PrintInstructions();
                    }
                    else
                    {
                        try
                        {
                            CliCommand command = (CliCommand)readStrings.First().Single();
                        }
                        catch (Exception test)
                        {
                            writer.Write(test.ToString());
                        }
                    }

                    checker.ExecuteSpellCheck("");
                    writer.Write(String.Join("\n", checker.GetAllIncorrectWords()));
                }
            }
        }

        public static void PrintInstructions()
        {

        }
    }
}
