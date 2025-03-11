//BY DX4D
#define LOG_MAIN_DIRECTORY_SCAN //uncomment to enable
#define LOG_PROCESSED_FILES //uncomment to enable
//#define LOG_DIRECTORY_SCAN //uncomment to enable
using System;
using System.IO;
using System.Text;

namespace symlinker
{
    internal class Program
    {
        const string LINK_PREFIX = "..";

        static int fileCount = 0;
        static StringBuilder output = new StringBuilder();
        static void Main(string[] args)
        {

            string path = Environment.CurrentDirectory;

#if LOG_MAIN_DIRECTORY_SCAN
            Log($"SYMLINKER BY DX4D\n\nScanning directory for symlinks...\n{path}\n\nUPDATED FILES:");
#endif

            Execute(path);
            PrintLog();
            WaitForKeyPress();
        }
        //EXECUTE
        static void Execute(string targetDirectory)
        {
#if LOG_DIRECTORY_SCAN
            Log($"\nScanning {targetDirectory}...");
#endif

            string[] files = Directory.GetFiles(targetDirectory);
            foreach (string file in files)
            {
                try
                {
                    if (ProcessFile(file, targetDirectory))
                    {
#if LOG_PROCESSED_FILES
                        Log(file);
#endif
                        fileCount++;
                    }
                }
                catch (Exception ex)
                {
                    Log($"ISSUE: Could not access {file}: {ex.Message}");
                }
            }

            foreach (string subfolder in Directory.GetDirectories(targetDirectory))
            {
                Execute(subfolder);
            }
        }

        static bool ProcessFile(string fileToProcess, string directory)
        {
            if (!IsValidFile(fileToProcess)) return false;

            string targetPath = ResolveRelativePath(fileToProcess, directory);
            string contents = File.ReadAllText(targetPath);
            File.WriteAllText(fileToProcess, contents);
            return true;
        }

        //IS VALID FILE
        static bool IsValidFile(string targetPath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(targetPath))
                {
                    char[] buffer = new char[2];
                    reader.Read(buffer, 0, 2);
                    return new string(buffer) == LINK_PREFIX;
                }
            }
            catch (Exception ex)
            {
                Log($"ISSUE: Could not access {targetPath}: {ex.Message}");
            }

            return false;
        }

        //RESOLVE RELATIVE PATH
        static string ResolveRelativePath(string relativePath, string directory)
        {
            string newPath = File.ReadAllText(relativePath).Trim();
            return Path.GetFullPath(Path.Combine(directory, newPath));
        }

        //DEBUG LOGGING
        static void Log(string toLog)
        {
            output.AppendLine(toLog);
        }
        static void PrintLog()
        {
            Console.WriteLine(output.ToString());
            Console.WriteLine($"Populated {fileCount} files with external content.");
        }
        static void WaitForKeyPress()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
