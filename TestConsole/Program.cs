using SubRealTeam.ConsoleUtility.Common.Logging;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.AddInstance(new ConsoleLogger());
            
            var testConfig = new TestConfiguration(args);
            if (testConfig.IsLogToFile)
            {
                Logger.AddInstance(new FileLogger());
            }
            
            if (testConfig.NotValidParameters)
            {
                Logger.Error("Not a valid parameters. Exit!");
                return;
            }
            
            Logger.SetLogLevelForInstance<FileLogger>(testConfig.LogLevel);

            testConfig.PrintHelp();

            Logger.Info("TestConsole was started.");

            Logger.Debug("TestConsole is in process.");

            var testInfo = testConfig.GetCommandLineArgumentInfo("logToFile");
            Logger.Info($"Argument {testInfo.Name} = {testInfo.Value}. Setup by default is {testInfo.SetupByDefault}");

            Logger.Info("TestConsole end process.");
        }
    }
}
