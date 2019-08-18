namespace RaspberryDjBot.Shell
{
    public struct ShellCommandResult
    {
        public bool Completed;
        public int? ExitCode;
        public string Output;
    }
}