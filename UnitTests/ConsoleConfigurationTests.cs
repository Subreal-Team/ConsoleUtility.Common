using NUnit.Framework;
using SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration;

namespace SubRealTeam.ConsoleUtility.Common.UnitTests
{
    [TestFixture]
    public class ConsoleConfigurationTests
    {
        private const string TestStringArg = "testString";
        private const char TestCharArg = 'c';
        public const char TestDefaultCharArg = 'q';
        private const bool TestBoolArg = false;
        public const bool TestDefaultBoolArg = true;
        private const int TestDigitBoolArg = 0;
        public const int TestDefaultIntArg = 10;
        private const decimal TestDecimalArg = 5432.1m;
        public const double TestDefaultDoubleArg = 1234.5;

        private static string[] TestArguments => new[]
        {
            "string=" + TestStringArg,
            "char=" + TestCharArg,
            "bool=" + TestBoolArg,
            "boolDigit=" + TestDigitBoolArg,
            "decimal=" + TestDecimalArg,
            // invalid argument string with decimal parameter
            "error=" + TestStringArg
        };

        [Test]
        public void TestConvert()
        {
            var testVal = "true";
            var testType = typeof(bool);

            var newVal = Convert.ChangeType(testVal, testType);
            Assert.IsTrue((bool)newVal);

            testVal = "10,";
            testType = typeof(int);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<FormatException>(() => Convert.ChangeType(testVal, testType));

            testType = typeof(decimal);
            newVal = Convert.ChangeType(testVal, testType);
            Assert.That((decimal)newVal, Is.EqualTo(10m));
        }


        [Test]
        public void WhenArgumentsIsValid_ShouldSetExpectedValues()
        {
            var testConfig = new TestConsoleConfiguration(TestArguments);
            Assert.Multiple(() =>
            {
                Assert.That(testConfig.StringArg, Is.EqualTo(TestStringArg));
                Assert.That(testConfig.CharArg, Is.EqualTo(TestCharArg));
                Assert.That(testConfig.BoolArg, Is.EqualTo(TestBoolArg));
                Assert.That(testConfig.BoolDigitArg, Is.EqualTo(TestBoolArg));
                Assert.That(testConfig.DecimalArg, Is.EqualTo(TestDecimalArg));
                Assert.That(testConfig.DoubleArg, Is.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.FloatArg, Is.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.ErrorArg, Is.Not.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.WithoutDefaultArg, Is.EqualTo(0));

                Assert.That(testConfig.NotValidParameters, Is.EqualTo(true));
                Assert.That(testConfig.NotValidParametersMessages, Has.Count.EqualTo(1));
            });
        }

        [Test]
        public void WhenBoolArgAsOneDigit_ShouldSetTrue()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "string=a", "bool=1" });
            Assert.Multiple(() =>
            {
                Assert.That(testConfig.NotValidParameters, Is.EqualTo(false));
                Assert.That(testConfig.BoolArg, Is.EqualTo(true));
            });
        }

        [Test]
        public void WhenBoolArgAsZeroDigit_ShouldSetFalse()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "string=a", "bool=0" });
            Assert.Multiple(() =>
            {
                Assert.That(testConfig.NotValidParameters, Is.False);
                Assert.That(testConfig.BoolArg, Is.EqualTo(false));
            });
        }

        [Test]
        public void WhenBoolArgNotValidDigit_ShouldSetDefault()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "bool=2" });
            Assert.Multiple(() =>
            {
                Assert.That(testConfig.NotValidParameters, Is.True);
                Assert.That(testConfig.BoolArg, Is.EqualTo(default(bool)));
            });
        }

        [Test]
        public void WhenArgumentsIsEmpty_ShouldReturnNoParametersSpecified()
        {
            var testConfig = new TestConsoleConfiguration(new string[] { });

            Assert.That(testConfig.NoParameters, Is.True);
        }

        [Test]
        public void WhenArgumentsIsNotDescribed_ShouldReturnDefault()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "string=a" });
            Assert.Multiple(() =>
            {
                Assert.That(testConfig.StringArg, Is.EqualTo("a"));
                Assert.That(testConfig.CharArg, Is.EqualTo(TestDefaultCharArg));
                Assert.That(testConfig.BoolArg, Is.EqualTo(TestDefaultBoolArg));
                Assert.That(testConfig.BoolDigitArg, Is.EqualTo(TestDefaultBoolArg));
                Assert.That(testConfig.DecimalArg, Is.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.DoubleArg, Is.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.FloatArg, Is.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.ErrorArg, Is.EqualTo(TestDefaultDoubleArg));
                Assert.That(testConfig.WithoutDefaultArg, Is.EqualTo(default(int)));

                Assert.That(testConfig.NotValidParametersMessages, Is.Empty);
                Assert.That(testConfig.NotValidParameters, Is.False);
            });
        }

        [Test]
        public void WhenRequiredArgumentNotSetup_ShouldAddNotValidParametersMessages()
        {
            var testConfig = new TestConsoleConfiguration();
            Assert.Multiple(() =>
            {
                Assert.That(testConfig.StringArg, Is.EqualTo(null));
                Assert.That(testConfig.NotValidParametersMessages, Is.Not.Empty);
                Assert.That(testConfig.NotValidParameters, Is.True);
            });
            Assert.That(testConfig.NotValidParametersMessages[0], Is.EqualTo("Attribute 'string' requires a value"));
        }

        [Test]
        public void TestPrintHelp()
        {
            static string DefaultValueDescription(object? value) =>
                value != null ? $" (default value is '{value}')" : string.Empty;

            var testConfig = new TestConsoleConfiguration(TestArguments);

            var helpMessage = testConfig.PrintHelp(false);

            Assert.That("string - StringArg" + DefaultValueDescription(null) + Environment.NewLine +
                "char - CharArg" + DefaultValueDescription(TestDefaultCharArg) + Environment.NewLine +
                "bool - BoolArg" + DefaultValueDescription(TestDefaultBoolArg) + Environment.NewLine +
                "boolDigit - BoolDigitArg" + DefaultValueDescription(TestDefaultBoolArg) + Environment.NewLine +
                "int - IntArg" + DefaultValueDescription(TestDefaultIntArg) + Environment.NewLine +
                "decimal - DecimalArg" + DefaultValueDescription(TestDefaultDoubleArg) + Environment.NewLine +
                "float - FloatArg" + DefaultValueDescription(TestDefaultDoubleArg) + Environment.NewLine +
                "double - DoubleArg" + DefaultValueDescription(TestDefaultDoubleArg) + Environment.NewLine +
                "error - ErrorArg" + DefaultValueDescription(TestDefaultDoubleArg) + Environment.NewLine + 
                "withoutdefault - WithoutDefaultArg", Is.EqualTo(helpMessage));
        }

        [Test]
        [TestCase("string", false), TestCase("String", false)]
        [TestCase("Char", false), TestCase("char", false)]
        [TestCase("Bool", false), TestCase("bool", false)]
        [TestCase("BoolDigit", false), TestCase("boolDigit", false)]
        [TestCase("Int", true), TestCase("int", true)]
        [TestCase("Decimal", false), TestCase("decimal", false)]
        [TestCase("Float", true), TestCase("float", true)]
        [TestCase("Double", true), TestCase("double", true)]
        [TestCase("Error", false), TestCase("error", false)]
        [TestCase("WithoutDefault", false), TestCase("withoutdefault", false)]
        public void TestGetCommandLineArgumentInfo(string argName, bool byDefault)
        {
            var testConfig = new TestConsoleConfiguration(TestArguments);

            var commandLineArgumentInfo = testConfig.GetCommandLineArgumentInfo(argName);
            Assert.Multiple(() =>
            {
                Assert.That(commandLineArgumentInfo, Is.Not.Null);
                Assert.That(string.Equals(argName, commandLineArgumentInfo.Name, StringComparison.OrdinalIgnoreCase), Is.True);
                Assert.That(commandLineArgumentInfo.SetupByDefault, Is.EqualTo(byDefault));
            });
        }
    }

    public class TestConsoleConfiguration : ConsoleConfigurationBase
    {
        public TestConsoleConfiguration()
        {
        }

        public TestConsoleConfiguration(string[]? arguments = null) : base(arguments)
        {
        }

        [CommandLineArgument("string", defaultValue: null, description: "StringArg")]
        [RequiredArgument]
        public string StringArg { get; set; }

        [CommandLineArgument("char", defaultValue: ConsoleConfigurationTests.TestDefaultCharArg, description: "CharArg")]
        public char CharArg { get; set; }

        [CommandLineArgument("bool", defaultValue: ConsoleConfigurationTests.TestDefaultBoolArg, description: "BoolArg")]
        public bool BoolArg { get; set; }

        [CommandLineArgument("boolDigit", defaultValue: ConsoleConfigurationTests.TestDefaultBoolArg, description: "BoolDigitArg")]
        public bool BoolDigitArg { get; set; }

        [CommandLineArgument("int", defaultValue: ConsoleConfigurationTests.TestDefaultIntArg, description: "IntArg")]
        public int IntArg { get; set; }

        [CommandLineArgument("decimal", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg,
            description: "DecimalArg")]
        public decimal DecimalArg { get; set; }

        [CommandLineArgument("float", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg,
            description: "FloatArg")]
        public float FloatArg { get; set; }

        [CommandLineArgument("double", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg,
            description: "DoubleArg")]
        public double DoubleArg { get; set; }

        [CommandLineArgument("error", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg,
            description: "ErrorArg")]
        public decimal ErrorArg { get; set; }

        [CommandLineArgument("withoutdefault", description: "WithoutDefaultArg")]
        public int WithoutDefaultArg { get; set; }
    }

    public class TestNotConsistentConfiguration : ConsoleConfigurationBase
    {
        [CommandLineArgument("list")]
        public List<string> ListArg { get; set; }
    }
}
