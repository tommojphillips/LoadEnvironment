using System;

namespace LoadINI
{
    public static class EnvVarTextWriter
    {
        public static void sendCmd(string var, string val)
        {
            // Used for expanding environment variables on leading lines.
            Environment.SetEnvironmentVariable(var, val);

            // Write the command to the std output.
            // A child process cannot set environment variables of a parent process.
            // So a workaround is to output the command to the output, and have the calling script run the command.
            Console.WriteLine("set \"" + var + "=" + val + "\"");
        }

        public static void echo(string msg = "\u0001")
        {
            if (msg == null)
                return;

            Console.Error.WriteLine(msg);
        }
    }
}
