using EzLogger;

namespace Ingestify
{
    internal enum CLI_ERR
    {
        OK,
        UNKNOWN,
        NO_INPUT_ARGS,
        INVALID_INPUT_ARG,
        INPUT_DIR_NOT_GIVEN,
        INPUT_DIRECTORY_NOT_FOUND,
        OUTPUT_DIR_NOT_GIVEN,
        IGNORE_PATH_NOT_GIVEN,
        IGNORE_FILE_NOT_FOUND,
    }

    internal class Inputs
    {
        public string InputDir       { get; set; } = string.Empty;
        public string OutputPath     { get; set; } = string.Empty;
        public string IgnoreFilePath { get; set; } = string.Empty;
    }

    internal class ArgsParser
    {
        
        public static CLI_ERR Parse(string[] args, Inputs inputs)
        {
            CLI_ERR ret = CLI_ERR.UNKNOWN;

            if (args.Length > 0)
            {
                string[] argsArray = RestructureArgs(args);

                for (int i = 0; i < argsArray.Length; i++)
                {
                    string[] splitString = argsArray[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    ret = splitString[0] switch
                    {
                        "in"      => ParseInputDir(splitString, inputs),
                        "out"     => ParseOutputPath(splitString, inputs),
                        "ignore"  => ParseIgnorePath(splitString, inputs),
                        _         => ParseInvalidArg(splitString)
                    };

                    if (ret != CLI_ERR.OK) break;
                }
            }

            else
                ret = CLI_ERR.NO_INPUT_ARGS;

            return ret;
        }

        private static string[] RestructureArgs(string[] args)
        {
            string combinedString = string.Join(" ", args);
            string[] argsArray = combinedString.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return argsArray;
        }

        private static CLI_ERR ParseInvalidArg(string[] splitString)
        {
            Logger.Error($"Invalid argument: {splitString[0]}");
            return CLI_ERR.INVALID_INPUT_ARG;
        }

        private static CLI_ERR ParseInputDir(string[] keyValuePair, Inputs inputs)
        {
            if (keyValuePair.Length < 1)                    return CLI_ERR.INPUT_DIR_NOT_GIVEN;
            if (Directory.Exists(keyValuePair[1]) == false) return CLI_ERR.INPUT_DIRECTORY_NOT_FOUND;
            inputs.InputDir = keyValuePair[1];
            return CLI_ERR.OK;
        }

        private static CLI_ERR ParseOutputPath(string[] keyValuePair, Inputs inputs)
        {
            if (keyValuePair.Length < 1)         return CLI_ERR.OUTPUT_DIR_NOT_GIVEN;
            inputs.OutputPath = keyValuePair[1];
            return CLI_ERR.OK;
        }

        private static CLI_ERR ParseIgnorePath(string[] keyValuePair, Inputs inputs)
        {
            if (keyValuePair.Length < 1)               return CLI_ERR.IGNORE_PATH_NOT_GIVEN;
            if (File.Exists(keyValuePair[1]) == false) return CLI_ERR.IGNORE_FILE_NOT_FOUND;
            inputs.IgnoreFilePath = keyValuePair[1];
            return CLI_ERR.OK;
        }
    }
}
