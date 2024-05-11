using System;
using System.IO;
using System.Text;

using static LoadINI.EnvVarTextWriter;

namespace LoadINI
{
    internal class Program
    {
        // Author, tommojphillips, 08.05.2024
        // Github, https://github.com/tommojphillips
        // Sets environment variables loaded from a .ini file.

        public const string AUTHOR = "by tommojphillips";
        public const string RELEASE_VER = "1";
        public const string MAJ_VER = "1";
        public const string MIN_VER = "0";
        public const string REV_VER = "0";
        public const string BLD_VER = "2";
        public const string PATCH_VER = "0";
        public const string COPYRIGHT = "2024";
        public const string PRODUCT_NAME = "Load Environment";
        public const string PRODUCT_DESC = "Sets environment variables read from a .ini file.";

        public const string FILE_VERSION = RELEASE_VER + ".0." + PRODUCT_VERSION + "." + PATCH_VER;
        public const string VERSION = MAJ_VER + "." + MIN_VER + "." + REV_VER + "." + BLD_VER;
        public const string PRODUCT_VERSION = MAJ_VER + MIN_VER + REV_VER + BLD_VER;

        static string name;
        static Parameters parameters;

        static void Main(string[] args)
        {
            name = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

            if (!parseArgs(args, out parameters))
                Environment.Exit(-1);

            LoadEnv ini = new LoadEnv(parameters);
            ini.load();
        }

        static bool parseArgs(string[] args, out Parameters parameters)
        {
            parameters = new Parameters();

            bool isPathSet = false;
            string arg;
            
            for (int i = 0; i < args.Length; i++)
            {
                arg = args[i];

                if (arg.StartsWith("-") || arg.StartsWith("/"))
                {
                    arg = arg.Remove(0, 1);

                    switch (arg)
                    {
                        case "q":
                            parameters.quiet = true;
                            break;

                        case "?":
                            goto Help;

                        default:
                            echo($"Error: unknown option '-{arg}'");
                            goto Usage;
                    }
                }
                else
                {
                    if (isPathSet)
                    {
                        echo("Error: too many arguments");
                        goto Usage;
                    }

                    isPathSet = true;

                    parameters.path = arg;
                }
            }

            if (!isPathSet)
            {
                echo("Error: a path to a .ini is required.");
                goto Usage;
            }

            if (!File.Exists(parameters.path))
            {
                echo($"Error: The .ini file could not be found, '{parameters.path}'.");
                return false;
            }

            return true;

            Usage:
                usage();
                return false;
            Help:
                help();
                return false;
        }

        static void usage()
        {
            echo($"\nUsage: {name} <ini> [-q]\n");
        }
        static void help()
        {
            StringBuilder sb = new StringBuilder();

            echo($"{PRODUCT_NAME} v{VERSION} - {AUTHOR} {COPYRIGHT}");

            usage();

            name += " <ini>";

            sb.AppendLine("This program is designed to be used in a batch script. (.bat .cmd)");
            sb.AppendLine(PRODUCT_DESC);
            sb.AppendLine("doesn't directly set the variable, but outputs the commands to do so.");
            sb.AppendLine();

            sb.AppendLine("Options:");
            sb.AppendLine("  -q     Suppresses all output except for errors.");
            sb.AppendLine();

            sb.AppendLine("Use a for loop to set the environment variables.");
            sb.AppendLine($"  --> for /f \"delims=\" %%k in ('{name}') do ( %%k )");

            sb.AppendLine("To suppress the std error, redirect it to nul.");
            sb.AppendLine($"  --> for /f \"delims=\" %%k in ('{name} 2^>nul') do ( %%k )");
            sb.AppendLine();

            sb.AppendLine("To log the std error, redirect it to a file.");
            sb.AppendLine($"  --> for /f \"delims=\" %%k in ('{name} 2^>error.txt') do ( %%k )");
            sb.AppendLine();

            sb.AppendLine("To define a variable in the .ini file, use the following format:");
            sb.AppendLine("  --> VAR=VAL");
            sb.AppendLine();

            sb.AppendLine("To undefine a variable, use the following format:");
            sb.AppendLine("  --> VAR=");
            sb.AppendLine();

            sb.AppendLine("To expand a variable, use the following format:");
            sb.AppendLine("vars you want to expand must be enclosed in '%'");
            sb.AppendLine("  --> VAR=%OTHER_VAR%");
            sb.AppendLine();

            sb.AppendLine("Comments can be used in the .ini file, they must start with ';'.");
            sb.Append("  --> ; this is a comment");

            echo(sb.ToString());
        }
    }
}
