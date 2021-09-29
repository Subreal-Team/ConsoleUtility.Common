using System;
using System.Reflection;

namespace SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration
{
    public class CommandLineArgumentInfo
    {
        public CommandLineArgumentInfo(PropertyInfo propertyInfo, bool setupByDefault, string name, object value)
        {
            PropertyInfo = propertyInfo;
            SetupByDefault = setupByDefault;
            Name = name;
            Value = value;
        }

        public PropertyInfo PropertyInfo { get; }

        public bool SetupByDefault { get; }

        public string Name { get; }

        public object Value { get; }
    }
}