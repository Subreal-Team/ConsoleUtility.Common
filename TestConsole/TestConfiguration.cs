using SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration;

namespace TestConsole
{
    internal class TestConfiguration : ConsoleConfigurationBase
    {
        // TODO - add any template processing, like this
        private const string ParseTemplate = "/{name}:{value}";

        [CommandLineArgument("LogToFile", defaultValue: true, description: "Log to test file")]
        public bool IsLogToFile { get; set; }
    }
}
