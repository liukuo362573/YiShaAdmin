using System.Diagnostics;

namespace YiSha.Util.Helper
{
    public static class ShellHelper
    {
        public static string Bash(string command)
        {
            var escapedArgs = command?.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        public static string Cmd(string fileName, string args)
        {
            var info = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(info);
            return process.StandardOutput.ReadToEnd();
        }
    }
}