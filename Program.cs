using EzLogger;
using GitIgnore = Ignore.Ignore;

namespace Ingestify
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Logger.SetConfig(Verbosity.Debug, Verbosity.Warning);
            try
            {
                CLI_ERR err = InnerMain(args);
                if (err == CLI_ERR.OK) Logger.Info($"App exiting, code: {err}, {(int)err}"); 
                else                   Logger.Error($"App exiting, code: {err}, {(int)err}"); 
                return (int)err;
            }
            catch (Exception ex)
            {
                Logger.Critical($"Exception thrown: {ex.Message}");
                Logger.Error(ex.StackTrace ?? "no stack information");
                return (int)CLI_ERR.UNKNOWN;
            }
            finally
            {
                Logger.StopLoggingTasks();
            }
        }

        private static CLI_ERR InnerMain(string[] args)
        {
            Inputs inputs = new();
            CLI_ERR ret = ArgsParser.Parse(args, inputs);
            if (ret != CLI_ERR.OK)
                return ret;

            var ignoreList = new GitIgnore();

            if (File.Exists(inputs.IgnoreFilePath))
            {
                var ignorePatterns = File.ReadAllLines(inputs.IgnoreFilePath);
                foreach (var pattern in ignorePatterns)
                {
                    ignoreList.Add(pattern);
                }
            }
            else
            {
                Logger.Warning("No ignore file has been provided! The output file might contain lots of garbage data.");
            }

            using (var writer = new StreamWriter(inputs.OutputPath))
            {
                WriteFilesRecursively(inputs.InputDir, inputs.InputDir, writer, ignoreList);
            }

            Logger.Info($"Files written to {Path.GetFullPath(inputs.OutputPath)}");

            return CLI_ERR.OK;
        }

        private static void WriteFilesRecursively(string rootPath, string currentPath, StreamWriter writer, GitIgnore ignoreList)
        {
            try
            {
                var files = Directory.GetFiles(currentPath);
                var directories = Directory.GetDirectories(currentPath);

                foreach (var file in files)
                {
                    var relativePath = Path.GetRelativePath(rootPath, file);
                    var unixRelativePath = relativePath.Replace(Path.DirectorySeparatorChar, '/');

                    if (!ignoreList.IsIgnored(unixRelativePath))
                    {
                        writer.WriteLine($"// file: {relativePath}:");
                        writer.WriteLine(File.ReadAllText(file));
                        writer.WriteLine();
                    }
                }

                foreach (var directory in directories)
                {
                    var relativeDirPath = Path.GetRelativePath(rootPath, directory);
                    var unixRelativeDirPath = relativeDirPath.Replace(Path.DirectorySeparatorChar, '/');

                    // Check if the entire directory is excluded
                    if (!ignoreList.IsIgnored(unixRelativeDirPath + "/"))
                    {
                        WriteFilesRecursively(rootPath, directory, writer, ignoreList);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing directory '{currentPath}': {ex.Message}");
                throw;
            }
        }
    }
}
