using NUnit.Framework;
using SubRealTeam.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubRealTeam.Common.UnitTests
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
