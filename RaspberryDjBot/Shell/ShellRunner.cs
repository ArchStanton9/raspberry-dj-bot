using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RaspberryDjBot.Shell
{
    public static class ShellRunner
    {
        private static ProcessStartInfo CreateStartInfo(string command, IEnumerable<string> args)
        {
            var info = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = command,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            foreach (var arg in args)
                info.ArgumentList.Add(arg);

            return info;
        }

        public static string RunCommand(string command, params string[] args)
        {
            var process = new Process
            {
                StartInfo = CreateStartInfo(command, args)
            };

            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        public static Task<string> RunCommandAsync(string command, params string[] args)
        {
            var tcs = new TaskCompletionSource<string>();
            var process = new Process
            {
                StartInfo = CreateStartInfo(command, args),
                EnableRaisingEvents = true
            };


            process.Exited += (sender, e) =>
            {
                var output = process.StandardOutput.ReadToEnd();
                tcs.SetResult(output);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
    }
}
