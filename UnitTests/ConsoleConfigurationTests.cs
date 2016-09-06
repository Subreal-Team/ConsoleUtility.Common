using System;
using System.Collections.Generic;
using NUnit.Framework;
using SubrealTeam.Common.ConsoleConfiguration;

namespace SubrealTeam.Common.UnitTests
{
	[TestFixture]
	public class ConsoleConfigurationTests
	{
		public static string TestStringArg = "testString";
		public static char TestCharArg = 'c';
		public static bool TestBoolArg = false;
		public static decimal TestDecimalArg = 5432.1m;
		public const double TestDefaultDoubleArg = 1234.5;

		[Test]
		[Ignore("")]
		public void TestConvert()
		{
			string testVal = "true";
			Type testType = typeof(bool);

			var newVal = Convert.ChangeType(testVal, testType);

			testVal = "10,";
			testType = typeof(int);
			newVal = Convert.ChangeType(testVal, testType);
		}
		

		[Test]
		public void TestGetArguments()
		{
			var testConfig = new TestConsoleConfiguration();

			Assert.AreEqual(testConfig.StringArg, TestStringArg);
			Assert.AreEqual(testConfig.CharArg, TestCharArg);
			Assert.AreEqual(testConfig.BoolArg, TestBoolArg);
			Assert.AreEqual(testConfig.DecimalArg, TestDecimalArg);
			Assert.AreEqual(testConfig.DoubleArg, TestDefaultDoubleArg);
			Assert.AreEqual(testConfig.FloatArg, TestDefaultDoubleArg);
			Assert.AreNotEqual(testConfig.ErrorArg, TestDefaultDoubleArg);
			Assert.AreEqual(testConfig.WithoutDefaultArg, 0);
			
			Assert.AreEqual(testConfig.NotValidParameters, true);
			Assert.AreEqual(testConfig.NotValidParamtersMessages.Count, 1);
		}
	}

	public class TestConsoleConfiguration : ConsoleConfigurationBase
	{
		public override void SetArguments()
		{
			_arguments = new[] { 
				"string=" + ConsoleConfigurationTests.TestStringArg, 
				"char=" + ConsoleConfigurationTests.TestCharArg, 
				"bool=" + ConsoleConfigurationTests.TestBoolArg,
				"decimal=" + ConsoleConfigurationTests.TestDecimalArg,
				// ошибочный аргумент строка с параметром decimal
				"error=" + ConsoleConfigurationTests.TestStringArg };
		}

		[CommandLineArgument("string", defaultValue: "default", description: "StringArg")]
		public string StringArg { get; set; }

		[CommandLineArgument("char", defaultValue: 'q', description: "CharArg")]
		public char CharArg { get; set; }

		[CommandLineArgument("bool", defaultValue: true, description: "BoolArg")]
		public bool BoolArg { get; set; }

		[CommandLineArgument("int", defaultValue: 10, description: "IntArg")]
		public int IntArg { get; set; }

		[CommandLineArgument("decimal", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg, description: "DecimalArg")]
		public decimal DecimalArg { get; set; }

		[CommandLineArgument("float", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg, description: "FloatArg")]
		public float FloatArg { get; set; }

		[CommandLineArgument("double", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg, description: "DoubleArg")]
		public double DoubleArg { get; set; }

		[CommandLineArgument("error", defaultValue: ConsoleConfigurationTests.TestDefaultDoubleArg, description: "DoubleArg")]
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
