using NUnit.Framework;
using SubRealTeam.ConsoleUtility.Common.Logging;

namespace SubRealTeam.ConsoleUtility.Common.UnitTests
{
    [TestFixture]
    public class LoggerTests
    {
        [OneTimeSetUp]
        public void TestSetUp()
        {
            Logger.AddInstance(new ConsoleLogger());
            Logger.AddInstance(new FileLogger(filePath: Environment.CurrentDirectory));
        }

        [Test]
        public void TestLogDebug()
        {
            Logger.SetLogLevelForInstance<FileLogger>(LogLevel.Debug);
            Logger.Debug("TEST DEBUG");
        }
    }
}
