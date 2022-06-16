using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ARLeF.Imprescj.DatabaseMigrator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string cs = @$"URI=file:{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/words.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DROP TABLE IF EXISTS words";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE Words(Key varchar(50) PRIMARY KEY, Value varchar(255))";
            cmd.ExecuteNonQuery();

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(@$"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/words.txt"))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    long lineCounter = 0;
                    (string, string) keyValuePairCache = default;

                    var timer = new Stopwatch();
                    timer.Start();

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (lineCounter <= 5) // Header lines
                        {
                            lineCounter++;
                            continue;
                        }
                        if (lineCounter % 2 == 0) // Even lines contain the key
                        {
                            keyValuePairCache.Item1 = Regex.Replace(line, "^ ", ""); // Remove whitespace at the beginning of the line
                        }
                        else // Odd lines contain the value
                        {
                            keyValuePairCache.Item2 = Regex.Replace(line, "^ ", ""); // Remove whitespace at the beginning of the line

                            // The key is added to the database
                            cmd.CommandText = "INSERT INTO Words(Key, Value) VALUES (@Key, @Value)";
                            cmd.Parameters.AddWithValue("@Key", keyValuePairCache.Item1);
                            cmd.Parameters.AddWithValue("@Value", keyValuePairCache.Item2);
                            cmd.ExecuteNonQuery();

                            keyValuePairCache = default;
                        }

                        Console.Write($"Elapsed: {TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds).TotalSeconds} seconds...");
                        Console.SetCursorPosition(0, Console.CursorTop);

                        lineCounter++;
                    }
                }
            }
        }
    }
}
