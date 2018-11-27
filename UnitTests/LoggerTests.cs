using NUnit.Framework;
using SubrealTeam.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubrealTeam.Common.UnitTests
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
