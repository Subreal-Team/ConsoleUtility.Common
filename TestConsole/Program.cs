using SubRealTeam.ConsoleUtility.Common.Logging;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var testConfig = new TestConfiguration();

            Logger.AddInstance(new ConsoleLogger());

            if (testConfig.IsLogToFile)
            {
                Logger.AddInstance(new FileLogger());
            }

            Logger.Info("TestConsole was started.");

            Logger.Debug("TestConsole is in process.");

            Logger.Info("TestConsole end process.");
        }
    }
}
