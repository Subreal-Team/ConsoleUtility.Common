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

            testConfig.PrintHelp();

            Logger.Info("TestConsole was started.");

            Logger.Debug("TestConsole is in process.");

            var testInfo = testConfig.GetCommandLineArgumentInfo("logToFile");
            Logger.Info($"Argument {testInfo.Name} = {testInfo.Value}. Setup by default is {testInfo.SetupByDefault}");

            Logger.Info("TestConsole end process.");
        }
    }
}
