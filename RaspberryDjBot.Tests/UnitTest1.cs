using NUnit.Framework;
using raspberry_dj_bot.Shell;

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
            var output = ShellCommandRunner.RunCommand("youtube-dl", "-e", "--get-duration", "-g",
                "https://www.youtube.com/watch?v=guaEK62mxno");

            var lines = output.Split("\n");
        }
    }
}