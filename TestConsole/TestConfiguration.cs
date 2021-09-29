using SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration;
using SubRealTeam.ConsoleUtility.Common.Logging;

namespace TestConsole
{
    internal class TestConfiguration : ConsoleConfigurationBase
    {
        // TODO - add any template processing, like this
        private const string ParseTemplate = "/{name}:{value}";

        public TestConfiguration()
        {
        }

        public TestConfiguration(string[] args) : base(args)
        {
        }

        [CommandLineArgument("LogToFile", defaultValue: true, description: "Log to test file")]
        public bool IsLogToFile { get; set; }
        
        [CommandLineArgument("LogLevel", description: "Log level for file log")]
        [RequiredArgument]
        public LogLevel LogLevel { get; set; }
    }
}
