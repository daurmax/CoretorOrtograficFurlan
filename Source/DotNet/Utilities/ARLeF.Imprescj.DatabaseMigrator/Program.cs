using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using LiteDB;

namespace ARLeF.Imprescj.DatabaseMigrator
{
    public enum FileType
    {
        Header,
        Regular,
        Tail
    }

    public class Program
    {
        private static readonly string WORDS_DB_PATH = @"words.db";
        private static readonly string WORDS_LOG_DB_PATH = @"words-log.db";
        private static readonly string WORDS_TEXT_FILES_FOLDER_PATH = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/WordsDatabase/TextFiles";

        public static void Main(string[] args)
        {
            File.Delete(WORDS_DB_PATH);
            File.Delete(WORDS_LOG_DB_PATH);

            using (var db = new LiteDatabase(WORDS_DB_PATH))
            {
                var globalTimer = new Stopwatch();
                globalTimer.Start();

                // Get words collection
                var wordsCollection = db.GetCollection<KeyValuePair<string, string>>("words");

                // Create unique index in key field
                wordsCollection.EnsureIndex(x => x.Key, true);

                // Use LINQ to query documents (with no index)
                //var results = col.Find(x => x.Key == "jnejew");
                var wordFilePaths = Directory.GetFiles(WORDS_TEXT_FILES_FOLDER_PATH)
                                           .ToList();

                // For estimating time remaining
                List<double> filesElapseds = new();

                wordFilePaths.Sort();
                
                /////
                Console.WriteLine("----------");

                ProcessTextFile(wordsCollection, wordFilePaths.First(), FileType.Header);

                var elapsedMilliseconds = globalTimer.ElapsedMilliseconds;
                filesElapseds.Add(elapsedMilliseconds);
                var estimatedRemainingMilliseconds = filesElapseds.Average() * (wordFilePaths.Count - 1);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{TimeSpan.FromMilliseconds(elapsedMilliseconds).TotalSeconds}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" seconds elapsed in total.");

                Console.Write("About ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{TimeSpan.FromMilliseconds(estimatedRemainingMilliseconds).Hours} hour(s), " +
                              $"{TimeSpan.FromMilliseconds(estimatedRemainingMilliseconds).Minutes} minute(s) and " +
                              $"{TimeSpan.FromMilliseconds(estimatedRemainingMilliseconds).Minutes} second(s)");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" remaining.");

                var fileLength = new FileInfo(WORDS_DB_PATH).Length;
                Console.Write("words.db filesize is ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(fileLength);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" bytes (");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write((double)fileLength / 1000000);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" megabytes).");

                var localTimer = new Stopwatch();
                localTimer.Start();

                for (int i = 1; i < wordFilePaths.Count; i++)
                {
                    Console.WriteLine("----------");

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"Processing file ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Regex.Replace(wordFilePaths[i], WORDS_TEXT_FILES_FOLDER_PATH + "/", ""));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($" ({i + 1} out of {wordFilePaths.Count})...");

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"File ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Regex.Replace(wordFilePaths[i], WORDS_TEXT_FILES_FOLDER_PATH + "/", ""));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($" ({i + 1} out of {wordFilePaths.Count})...");

                    ProcessTextFile(wordsCollection, wordFilePaths[i], FileType.Regular);

                    elapsedMilliseconds = localTimer.ElapsedMilliseconds;
                    filesElapseds.Add(elapsedMilliseconds);
                    localTimer.Restart();
                    estimatedRemainingMilliseconds = filesElapseds.Average() * (wordFilePaths.Count - i);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{TimeSpan.FromMilliseconds(elapsedMilliseconds).TotalSeconds}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" seconds elapsed in total.");

                    Console.Write("About ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{TimeSpan.FromMilliseconds(estimatedRemainingMilliseconds).Hours} hour(s), " +
                                  $"{TimeSpan.FromMilliseconds(estimatedRemainingMilliseconds).Minutes} minute(s) and " +
                                  $"{TimeSpan.FromMilliseconds(estimatedRemainingMilliseconds).Minutes} second(s)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" remaining.");

                    fileLength = new FileInfo(WORDS_DB_PATH).Length;
                    Console.Write("words.db filesize is ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(fileLength);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" bytes (");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write((double)fileLength / 1000000);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" megabytes).");
                }

                Console.WriteLine("----------");

                ProcessTextFile(wordsCollection, wordFilePaths.Last(), FileType.Tail);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{TimeSpan.FromMilliseconds(globalTimer.ElapsedMilliseconds).TotalSeconds}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" seconds elapsed in total.");

                fileLength = new FileInfo(WORDS_DB_PATH).Length;
                Console.Write("words.db filesize is ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(fileLength);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" bytes (");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write((double)fileLength / 1000000);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(") megabytes).");

                globalTimer.Stop();
                Console.WriteLine("----------");
                Console.WriteLine($"KeyValue database words.db processed in {globalTimer.Elapsed}.");
            }
        }

        public static void ProcessTextFile(ILiteCollection<KeyValuePair<string, string>> collection, string path, FileType fileType)
        {
            long lineCounter = 0;

            var timer = new Stopwatch();
            timer.Start();

            string[] textFileLines = File.ReadAllLines(path);
            List<KeyValuePair<string, string>> keyValuePairs = new();

            int startingPoint = fileType == FileType.Regular ? 0 : 6; // Skip header lines in header file
            int endingPoint = fileType == FileType.Regular ? textFileLines.Count() - 1 : textFileLines.Count() - 2; // Skip footer line in tail file

            for (int i = startingPoint; i < endingPoint; i = i + 2)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(textFileLines[i], textFileLines[i + 1]));

                lineCounter++;
            }

            collection.Insert(keyValuePairs);
        }
    }
}
