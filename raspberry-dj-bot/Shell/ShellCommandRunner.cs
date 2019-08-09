using System.Diagnostics;

namespace raspberry_dj_bot.Shell
{
    public static class ShellCommandRunner
    {
        public static string RunCommand(string command, params string[] args)
        {
            // Start the child process.
            var p = new Process();


            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = command;

            foreach (var arg in args)
            {
                p.StartInfo.ArgumentList.Add(arg);
            }

            p.Start();
            var output = p.StandardOutput.ReadToEnd();

            p.WaitForExit();

            return output;
        }
    }
}
