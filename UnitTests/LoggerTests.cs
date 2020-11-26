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
            
        }

        [Test]
        public void TestLogDebug()
        {
            Logger.AddInstance(new ConsoleLogger());
            Logger.AddInstance(new FileLogger());
        }
    }
}
