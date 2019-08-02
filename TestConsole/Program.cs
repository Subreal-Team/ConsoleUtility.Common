using SubRealTeam.ConsoleUtility.Common.Logging;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.AddInstance(new ConsoleLogger());
            Logger.AddInstance(new FileLogger());

            Logger.Info("TestConsole was started.");
        }
    }
}
