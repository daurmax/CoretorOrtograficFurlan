using ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.Struments.CoretorOrtografic.Core.Input;
using ARLeF.Struments.CoretorOrtografic.Core.SpellChecker;
using Autofac;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using ARLeF.Struments.Components.CoretorOrtografic.Entities.ProcessedElements;

namespace ARLeF.Struments.CoretorOrtografic.CLI
{
    class Program
    {
        private static IContainer Container { get; set; }

        private static IContentReader Reader { get; set; }

        private static ISpellChecker Checker { get; set; }

        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Container = CoretorOrtograficCliDependencyContainer.Configure
#if DEBUG
                (true);
#else
                (false);
#endif

            using (var scope = Container.BeginLifetimeScope())
            {
                Reader = scope.Resolve<IContentReader>();

                Checker = scope.Resolve<ISpellChecker>();

                while (true)
                {
                    var readStrings = Reader.Read()?.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (readStrings is null || !readStrings.Any())
                    {
                        Console.WriteLine("No commands or words were provided.");
                        PrintInstructions();
                    }
                    else if (readStrings.First().ToUpper() == "Q" || readStrings.First().ToUpper() == "QUIT")
                    {
                        Console.WriteLine("Closing application...");
                        Environment.Exit(1);
                    }
                    else if (readStrings.Count == 1)
                    {
                        Console.WriteLine("Please provide a command and at least a word to check.");
                        PrintInstructions();
                    }
                    else if (readStrings.First().Length != 1)
                    {
                        Console.WriteLine($"Unknown command '{readStrings.First()}'.");
                        PrintInstructions();
                    }
                    else
                    {
                        try 
                        {
                            switch (Char.ToUpper(readStrings.First().Single()))
                            {
                                case 'C':
                                    PrintWordsCorrectness(readStrings.Skip(1).ToList());
                                    break;
                                case 'S':
                                    PrintSuggestedWords(readStrings.Skip(1).ToList());
                                    break;
                                default:
                                    Console.WriteLine($"Unknown command '{readStrings.First()}'.");
                                    PrintInstructions();
                                    break;
                            }
                        }
                        catch (Exception test)
                        {
                            Console.WriteLine(test.ToString());
                        }
                    }
                }
            }
        }

        public static void PrintInstructions()
        {

        }
        private static void PrintWordsCorrectness(List<string> words)
        {
            Checker.ExecuteSpellCheck(String.Join(" ", words));

            foreach (ProcessedWord processedWord in Checker.ProcessedWords)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{processedWord.Original}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" is ");
                if (processedWord.Correct)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("correct");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(".");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("incorrect");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(".");
                }
            }

            Console.WriteLine();
            Checker.CleanSpellChecker();
        }
        private static void PrintSuggestedWords(List<string> words)
        {
            Checker.ExecuteSpellCheck(String.Join(" ", words));
        }
    }
}
