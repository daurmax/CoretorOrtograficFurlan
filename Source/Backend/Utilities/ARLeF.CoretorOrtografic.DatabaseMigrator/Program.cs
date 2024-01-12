using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ARLeF.DatabaseMigrator
{
    public enum FileType
    {
        Header,
        Regular = default,
        Tail
    }

    public class Program
    {
        private static readonly string ELISIONS_DB_PATH = @"elisions.db";

        private static readonly string ERRORS_DB_PATH = @"errors.db";

        private static readonly string FREC_DB_PATH = @"frec.db";

        private static readonly string WORDS_DB_PATH = @"words.db";

        private static readonly string ELISIONS_TEXT_FILES_FOLDER_PATH = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Elisions/TextFiles";
        private static readonly string ERRORS_TEXT_FILES_FOLDER_PATH = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Errors/TextFiles";
        private static readonly string FREC_TEXT_FILES_FOLDER_PATH = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Frec/TextFiles";
        private static readonly string WORDS_TEXT_FILES_FOLDER_PATH = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/WordsDatabase/TextFiles";

        public static void Main(string[] args)
        {
            ProcessDatabase(ELISIONS_TEXT_FILES_FOLDER_PATH, ELISIONS_DB_PATH);
            ProcessDatabase(FREC_TEXT_FILES_FOLDER_PATH, FREC_DB_PATH);
            ProcessDatabase(ERRORS_TEXT_FILES_FOLDER_PATH, ERRORS_DB_PATH);
            ProcessDatabase(WORDS_TEXT_FILES_FOLDER_PATH, WORDS_DB_PATH);
        }

        private static void ProcessDatabase(string textFilesFolderPath, string dbPath)
        {
            File.Delete(dbPath);

            var globalTimer = new Stopwatch();
            globalTimer.Start();

            var filePaths = Directory.GetFiles(textFilesFolderPath).ToList();

            // For estimating time remaining
            List<double> filesElapseds = new();

            filePaths.Sort();

            var localTimer = new Stopwatch();
            localTimer.Start();

            List<KeyValuePair<string, string>> keyValuePairs = new();

            for (int i = 0; i < filePaths.Count; i++)
            {
                FileType fileType = FileType.Regular;

                Console.WriteLine("----------");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"Processing file ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Path.GetFileName(filePaths[i]));
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($" ({i + 1} out of {filePaths.Count})...");

                keyValuePairs.AddRange(ProcessTextFile(filePaths[i], fileType));

                long elapsedMilliseconds = localTimer.ElapsedMilliseconds;
                filesElapseds.Add(elapsedMilliseconds);
                localTimer.Restart();
                double estimatedRemainingTicks = filesElapseds.Average() * (filePaths.Count - i);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Path.GetFileName(filePaths[i]));
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
            Console.Write($"Files processed in ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{TimeSpan.FromMilliseconds(globalTimer.ElapsedMilliseconds).TotalSeconds}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" seconds.");
            Console.WriteLine("--------------------");

            Console.WriteLine($"Creating {Path.GetFileName(dbPath)}...");
            localTimer.Restart();

            string cs = @$"URI=file:{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/{Path.GetFileName(dbPath)}";

            using (var con = new SQLiteConnection(cs))
            {
                con.Open();

                using var cmd = new SQLiteCommand(con);

                cmd.CommandText = "DROP TABLE IF EXISTS Data";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE Data(Key varchar(50) PRIMARY KEY, Value varchar(255))";
                cmd.ExecuteNonQuery();

                using (var transaction = con.BeginTransaction())
                {
                    var command = con.CreateCommand();

                    command.CommandText = "INSERT INTO Data(Key, Value) VALUES (@Key, @Value)";

                    foreach (var keyValuePair in keyValuePairs)
                    {
                        command.Parameters.AddWithValue("@Key", keyValuePair.Key);
                        command.Parameters.AddWithValue("@Value", keyValuePair.Value);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{TimeSpan.FromMilliseconds(localTimer.ElapsedMilliseconds).TotalSeconds}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" seconds elapsed.");

            var fileLength = new FileInfo(dbPath).Length;
            Console.Write($"{Path.GetFileName(dbPath)} filesize is ");
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
            Console.WriteLine($"KeyValue database {Path.GetFileName(dbPath)} processed in {globalTimer.Elapsed}.");
        }
        private static IEnumerable<KeyValuePair<string, string>> ProcessTextFile(string path, FileType fileType)
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
