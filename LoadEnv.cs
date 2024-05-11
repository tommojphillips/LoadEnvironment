using System;
using System.IO;
using System.Text.RegularExpressions;
using static LoadINI.EnvVarTextWriter;

namespace LoadINI
{
    public class LoadEnv
    {
        // Author, tommojphillips, 08.05.2024
        // Github, https://github.com/tommojphillips
        // Sets environment variables loaded from a .ini file.
        // EG
        //    --> VS_CODE=C:\Program Files\Microsoft VS Code\Code.exe
        //    --> PATH=%PATH%;%VS_CODE%

        private const string ENTRY_PATTERN = @"(\w+)\s*=\s*(.*)";

        private Parameters args;

        public LoadEnv(Parameters args)
        {
            this.args = args;
        }

        public bool read(out string[] lines)
        {
            lines = null;

            try
            { 
                lines = File.ReadAllLines(args.path);
            }
            catch (Exception e)
            {
                echo("Error reading file: " + e.Message);
                return false;
            }

            return true;
        }

        public void load()
        {
            string[] lines;
            string line;
            string lineTrimmed;
            string var;
            string val;
            Match match;

            if (!read(out lines))
            {
                return;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i].Replace("!", "%");
                lineTrimmed = line.Trim();

                if (lineTrimmed.Length == 0) continue;
                if (lineTrimmed[0] == ';') continue; // line is a comment.

                // line format: VAR=VAL
                if (!line.Contains("="))
                {
                    echo($"Error: '{line}' is missing an assignment operator. ( line: {i + 1} )");
                    continue;
                }

                match = Regex.Match(line, ENTRY_PATTERN);

                if (match.Success)
                {
                    var = match.Groups[1].Value;
                    val = match.Groups[2].Value;
                }
                else
                {
                    echo($"Error: '{line}' is not in the correct format. ( line: {i + 1} )");
                    continue;
                }

                /*string[] parts = line.Split('=');

                if (parts.Length > 0)
                {
                    var = parts[0];
                    
                    if (parts.Length > 1)
                        val = parts[1];
                    else
                        val = "";
                }
                else
                {
                    echo($"Error: '{line}' is missing a variable name. ( line: {i + 1} )");
                    continue;
                }*/


                /*bool lineError = false;
                string[] varsToExpandInLine = line.Split('%');

                for (int j = 1; j < varsToExpandInLine.Length; j++)
                {
                    string subVar = varsToExpandInLine[j];
                    string subVarExp = "%" + subVar + "%";
                    string subValExp = Environment.ExpandEnvironmentVariables(subVarExp);

                    if (subValExp == subVarExp)
                    {
                        echo($"Error setting var '{var}'. '{subVar}' is not defined. ( line: {i+1} )");
                        lineError = true;
                        break;
                    }

                    val = val.Replace(subVarExp, subValExp);

                    j++; // skip even indexes. as vars are enclosed in '%'.
                }

                if (lineError)
                    continue;*/

                if (!expandVars(ref val))
                {
                    echo($"Error setting var '{var}'. '{val}' is not defined. ( line: {i + 1} )");
                    continue;
                }

                sendCmd(var, val);

                // echo the command to the console.
                if (!args.quiet) echo($"{var}={val}");
            }
        }

        private bool expandVars(ref string val)
        {
            string[] varsToExpandInLine = val.Split('%');
            string curVar;
            string curVarExp;
            string curVal;

            for (int j = 1; j < varsToExpandInLine.Length; j++)
            {
                curVar = varsToExpandInLine[j];
                curVarExp = "%" + curVar + "%";
                curVal = Environment.ExpandEnvironmentVariables(curVarExp);

                if (curVal == curVarExp)
                {
                    val = curVar;
                    return false;
                }

                val = val.Replace(curVarExp, curVal);

                j++; // skip even indexes. as vars are enclosed in '%'.
            }
            
            return true;
        }
    }
}
