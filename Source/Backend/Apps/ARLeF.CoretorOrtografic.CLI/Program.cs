using ARLeF.CoretorOrtografic.Infrastructure.SpellChecker;
using ARLeF.CoretorOrtografic.Core.Input;
using ARLeF.CoretorOrtografic.Core.SpellChecker;
using Autofac;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using ARLeF.Components.CoretorOrtografic.Entities.ProcessedElements;

namespace ARLeF.CoretorOrtografic.CLI
{
    public class Program
    {
        private static IContainer _container;
        private static IContentReader _reader;
        private static ISpellChecker _checker;

        public static void Main(string[] args)
        {

            _container = CoretorOrtograficCliDependencyContainer.Configure
#if DEBUG
                (true);
#else
                (false);
#endif

            Console.ForegroundColor = ConsoleColor.White;

            PrintTitle();
            PrintInstructions();

            using (var scope = _container.BeginLifetimeScope())
            {
                _reader = scope.Resolve<IContentReader>();

                _checker = scope.Resolve<ISpellChecker>();

                while (true)
                {
                    var readStrings = _reader.Read()?.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

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
                }
            }
        }

        public static void PrintTitle()
        {
            Console.Title = "ASCII Art";
            string title = @"
                                                                                      
           ~!!~             .!!!!!!!!!!!!~~^.         :!!!!.      ^7JJ?^.     .!!!!!!!!!!!!!!!!!!!!.
          Y@@@@5            ?@@@@@@@@@@@@@@@@&G!      P@@@@?    Y&@&BG#@&P.   ~@@@@@@@@@@@@@@@@@@@@5
         ^@@@@@@^           ?@@@@&GGGGGGGG#&@@@@&!    P@@@@?   G@@7    :&@&.  ~@@@@&GGGGGGGGGGGGGGG!
         #@@@@@@#           ?@@@@G         .?&@@@@?   P@@@@7   &@&GGGGGG&@@^  ~@@@@B                
        Y@@@&&@@@Y          ?@@@@G           ^@@@@&   P@@@@7   &@&7!!!!!~~~   ~@@@@B                
       :@@@@?7@@@@^         ?@@@@G           .&@@@@.  P@@@@7   #@&:     ^.    ~@@@@B                
       #@@@&  #@@@#         ?@@@@P           Y@@@@#   P@@@@7   ^&@@P77Y&@&!   ~@@@@B                
      Y@@@@!  !@@@@Y        ?@@@@B.......:^J#@@@@B.   P@@@@7     !5B##BP7.    ~@@@@#:............   
     :@@@@B    B@@@@^       ?@@@@@@@@@@@@@@@@@@#7     P@@@@7                  ~@@@@@@@@@@@@@@@@@#   
     #@@@@^    :@@@@#       ?@@@@&&&&&&&&@@@@&:       P@@@@7                  ~@@@@@&&&&&&&&&&&&B   
    J@@@@5      P@@@@Y      ?@@@@G      ^&@@@@7       P@@@@7                  ~@@@@B                
   :&@@@@GJJJJJJG@@@@@^     ?@@@@P       7@@@@&:      P@@@@7                  ~@@@@B                
   B@@@@@@@@@@@@@@@@@@#     ?@@@@G        5@@@@#      P@@@@7                  ~@@@@B                
  J@@@@&YYYYYYYYYY#@@@@J    ?@@@@G         #@@@@P     P@@@@7                  ~@@@@B                
 :&@@@@^          ^@@@@@:   ?@@@@G         :&@@@@7    P@@@@7                  ~@@@@B                
 B@@@@5            5@@@@B   ?@@@@G          !@@@@&^   P@@@@#555555555555555.  ~@@@@B                
5@@@@&.             &@@@@Y  ?@@@@G           5@@@@&.  G@@@@@@@@@@@@@@@@@@@@:  ~@@@@#                
7????^              :????7  .????^            7????.  ^???????????????????7   .????~                
                                                                                                    
        ";

            Console.WriteLine(title);
        }
        public static void PrintInstructions()
        {

        }
        private static void PrintWordsCorrectness(List<string> words)
        {
            try
            {
                _checker.ExecuteSpellCheck(String.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception of type {ex.GetType()} occurred.");
            }
            finally
            {
                Console.WriteLine();
                _checker.CleanSpellChecker();
            }
        }
        private static void PrintSuggestedWords(List<string> words)
        {
            try
            {
                _checker.ExecuteSpellCheck(String.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
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
                        Console.Write(". ");
                        var suggestedWords = _checker.GetWordSuggestions(processedWord).Result;
                        if (suggestedWords is null || !suggestedWords.Any())
                        {
                            Console.WriteLine("There are no suggestions.");
                        }
                        else
                        {
                            Console.Write("Suggestions are: ");
                            foreach (var suggestedWord in suggestedWords)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(suggestedWord);
                                if (suggestedWord != suggestedWords.Last())
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(", ");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(".");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception of type {ex.GetType()} occurred.");
            }
            finally
            {
                Console.WriteLine();
                _checker.CleanSpellChecker();
            }
        }
    }
}
