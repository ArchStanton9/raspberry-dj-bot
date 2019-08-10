using System.Threading.Tasks;
using NUnit.Framework;
using RaspberryDjBot.Shell;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var output = ShellRunner.RunCommand("youtube-dl", "-e", "--get-duration", "-g",
                "https://www.youtube.com/watch?v=guaEK62mxno");

            var lines = output.Split("\n");
        }

        [Test]
        public async Task Can_run_command_async()
        {
            var output = await ShellRunner.RunCommandAsync("youtube-dl", "-e", "--get-duration", "-g",
                "https://www.youtube.com/watch?v=guaEK62mxno");

            var lines = output.Split("\n");
        }
    }
}