using System;
using System.Collections.Generic;
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
            Assert.AreEqual(10m, (decimal)newVal);
        }


        [Test]
        public void WhenArgumentsIsValid_ShouldSetExpectedValues()
        {
            var testConfig = new TestConsoleConfiguration(TestArguments);

            Assert.AreEqual(testConfig.StringArg, TestStringArg);
            Assert.AreEqual(testConfig.CharArg, TestCharArg);
            Assert.AreEqual(testConfig.BoolArg, TestBoolArg);
            Assert.AreEqual(testConfig.BoolDigitArg, TestBoolArg);
            Assert.AreEqual(testConfig.DecimalArg, TestDecimalArg);
            Assert.AreEqual(testConfig.DoubleArg, TestDefaultDoubleArg);
            Assert.AreEqual(testConfig.FloatArg, TestDefaultDoubleArg);
            Assert.AreNotEqual(testConfig.ErrorArg, TestDefaultDoubleArg);
            Assert.AreEqual(testConfig.WithoutDefaultArg, 0);

            Assert.AreEqual(testConfig.NotValidParameters, true);
            Assert.AreEqual(testConfig.NotValidParametersMessages.Count, 1);
        }

        [Test]
        public void WhenBoolArgAsOneDigit_ShouldSetTrue()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "string=a", "bool=1" });

            Assert.AreEqual(testConfig.NotValidParameters, false);
            Assert.AreEqual(testConfig.BoolArg, true);
        }

        [Test]
        public void WhenBoolArgAsZeroDigit_ShouldSetFalse()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "string=a", "bool=0" });

            Assert.IsFalse(testConfig.NotValidParameters);
            Assert.AreEqual(false, testConfig.BoolArg);
        }

        [Test]
        public void WhenBoolArgNotValidDigit_ShouldSetDefault()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "bool=2" });

            Assert.IsTrue(testConfig.NotValidParameters);
            Assert.AreEqual(default(bool), testConfig.BoolArg);
        }

        [Test]
        public void WhenArgumentsIsEmpty_ShouldReturnNoParametersSpecified()
        {
            var testConfig = new TestConsoleConfiguration(new string[] { });

            Assert.IsTrue(testConfig.NoParameters);
        }

        [Test]
        public void WhenArgumentsIsNotDescribed_ShouldReturnDefault()
        {
            var testConfig = new TestConsoleConfiguration(new[] { "string=a" });

            Assert.AreEqual("a", testConfig.StringArg);
            Assert.AreEqual(TestDefaultCharArg, testConfig.CharArg);
            Assert.AreEqual(TestDefaultBoolArg, testConfig.BoolArg);
            Assert.AreEqual(TestDefaultBoolArg, testConfig.BoolDigitArg);
            Assert.AreEqual(TestDefaultDoubleArg, testConfig.DecimalArg);
            Assert.AreEqual(TestDefaultDoubleArg, testConfig.DoubleArg);
            Assert.AreEqual(TestDefaultDoubleArg, testConfig.FloatArg);
            Assert.AreEqual(TestDefaultDoubleArg, testConfig.ErrorArg);
            Assert.AreEqual(default(int), testConfig.WithoutDefaultArg);

            Assert.IsEmpty(testConfig.NotValidParametersMessages);
            Assert.IsFalse(testConfig.NotValidParameters);
        }
        
        [Test]
        public void WhenRequiredArgumentNotSetup_ShouldAddNotValidParametersMessages()
        {
            var testConfig = new TestConsoleConfiguration();

            Assert.AreEqual(null, testConfig.StringArg);

            Assert.IsNotEmpty(testConfig.NotValidParametersMessages);
            Assert.IsTrue(testConfig.NotValidParameters);
            Assert.AreEqual("Attribute 'string' requires a value", testConfig.NotValidParametersMessages[0]);
        }

        [Test]
        public void TestPrintHelp()
        {
            static string DefaultValueDescription(object value) =>
                value != null ? $" (default value is '{value}')" : string.Empty;

            var testConfig = new TestConsoleConfiguration(TestArguments);

            var helpMessage = testConfig.PrintHelp(false);

            Assert.AreEqual(helpMessage,
                "string - StringArg" + DefaultValueDescription(null) + "\r\n" +
                "char - CharArg" + DefaultValueDescription(TestDefaultCharArg) + "\r\n" +
                "bool - BoolArg" + DefaultValueDescription(TestDefaultBoolArg) + "\r\n" +
                "boolDigit - BoolDigitArg" + DefaultValueDescription(TestDefaultBoolArg) + "\r\n" +
                "int - IntArg" + DefaultValueDescription(TestDefaultIntArg) + "\r\n" +
                "decimal - DecimalArg" + DefaultValueDescription(TestDefaultDoubleArg) + "\r\n" +
                "float - FloatArg" + DefaultValueDescription(TestDefaultDoubleArg) + "\r\n" +
                "double - DoubleArg" + DefaultValueDescription(TestDefaultDoubleArg) + "\r\n" +
                "error - ErrorArg" + DefaultValueDescription(TestDefaultDoubleArg) + "\r\n" + 
                "withoutdefault - WithoutDefaultArg");
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
            
            Assert.IsNotNull(commandLineArgumentInfo);
            Assert.IsTrue(string.Equals(argName, commandLineArgumentInfo.Name, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(byDefault, commandLineArgumentInfo.SetupByDefault);
        }
    }

    public class TestConsoleConfiguration : ConsoleConfigurationBase
    {
        public TestConsoleConfiguration()
        {
        }

        public TestConsoleConfiguration(string[] arguments = null) : base(arguments)
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
