using System.Data.SQLite;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using LiteDB;

namespace ARLeF.Imprescj.DatabaseMigrator
{
    public enum FileType
    {
        Header,
        Regular = default,
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
                var wordsCollection = db.GetCollection<BsonDocument>("words");

                // Create unique index in key field
                wordsCollection.EnsureIndex("_id");

                var wordFilePaths = Directory.GetFiles(WORDS_TEXT_FILES_FOLDER_PATH).ToList();

                // For estimating time remaining
                List<double> filesElapseds = new();

                wordFilePaths.Sort();
                //wordsCollection.Insert(ProcessTextFile(wordFilePaths.First(), FileType.Header));

                var localTimer = new Stopwatch();
                localTimer.Start();

                List<KeyValuePair<string, string>> keyValuePairs = new();

                for (int i = 0; i < wordFilePaths.Count; i++)
                {
                    FileType fileType = FileType.Regular;

                    Console.WriteLine("----------");

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"Processing file ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Regex.Replace(wordFilePaths[i], WORDS_TEXT_FILES_FOLDER_PATH + "/", ""));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($" ({i + 1} out of {wordFilePaths.Count})...");

                    keyValuePairs.AddRange(ProcessTextFile(wordFilePaths[i], fileType));

                    long elapsedMilliseconds = localTimer.ElapsedMilliseconds;
                    filesElapseds.Add(elapsedMilliseconds);
                    localTimer.Restart();
                    double estimatedRemainingTicks = filesElapseds.Average() * (wordFilePaths.Count - i);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Regex.Replace(wordFilePaths[i], WORDS_TEXT_FILES_FOLDER_PATH + "/", ""));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" processed in ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{TimeSpan.FromMilliseconds(elapsedMilliseconds).TotalMilliseconds}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" milliseconds.");

                    Console.Write("About ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{TimeSpan.FromMilliseconds((long)estimatedRemainingTicks).TotalSeconds} second(s)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" remaining.");

                    Console.WriteLine("----------");

                    Console.SetCursorPosition(0, Console.CursorTop - 5);
                }
                Console.Clear();
                Console.WriteLine("--------------------");
                Console.Write($"Word files processed in ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{TimeSpan.FromMilliseconds(globalTimer.ElapsedMilliseconds).TotalSeconds}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" seconds.");
                Console.WriteLine("--------------------");

                Console.WriteLine($"Creating word.db...");
                localTimer.Restart();

                string cs = @$"URI=file:{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/words.db";

                using (var con = new SQLiteConnection(cs))
                {
                    con.Open();

                    using var cmd = new SQLiteCommand(con);

                    cmd.CommandText = "DROP TABLE IF EXISTS Words";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE Words(Key varchar(50) PRIMARY KEY, Value varchar(255))";
                    cmd.ExecuteNonQuery();

                    using (var transaction = con.BeginTransaction())
                    {
                        var command = con.CreateCommand();

                        command.CommandText = "INSERT INTO Words(Key, Value) VALUES (@Key, @Value)";
                        
                        foreach (var keyValuePair in keyValuePairs)
                        {
                            command.Parameters.AddWithValue("@Key", keyValuePair.Key);
                            command.Parameters.AddWithValue("@Value", keyValuePair.Value);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                //wordsCollection.DeleteAll();
                //wordsCollection.InsertBulk(keyValuePairs);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{TimeSpan.FromMilliseconds(localTimer.ElapsedMilliseconds).TotalSeconds}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" seconds elapsed.");
                
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

                globalTimer.Stop();
                Console.WriteLine("----------");
                Console.WriteLine($"KeyValue database words.db processed in {globalTimer.Elapsed}.");
            }
        }

        public static IEnumerable<KeyValuePair<string, string>> ProcessTextFile(string path, FileType fileType)
        {
            var timer = new Stopwatch();
            timer.Start();

            string[] textFileLines = File.ReadAllLines(path);
            List<KeyValuePair<string, string>> keyValuePairs = new();

            int startingPoint = 0;
            int endingPoint = textFileLines.Count() - 1;
            if (fileType == FileType.Header) 
            {
                startingPoint = 6; // Skip header lines in header file
            }
            else if (fileType == FileType.Tail)
            {
                endingPoint = textFileLines.Count() - 2; // Skip footer line in tail file
            }

            for (int i = startingPoint; i < endingPoint; i = i + 2)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(Regex.Replace(textFileLines[i], "^ ", ""),
                                                                   Regex.Replace(textFileLines[i + 1], "^ ", "")));
            }

            return keyValuePairs;
        }
    }
}
