using System;
using System.Reflection;

namespace SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration
{
    /// <summary>
    /// Information about command line argument described in ConsoleConfiguration class
    /// </summary>
    public sealed class CommandLineArgumentInfo
    {
        /// <summary>
        /// Construct CommandLineArgumentInfo
        /// </summary>
        /// <param name="propertyInfo">Described argument Property Info </param>
        /// <param name="setupByDefault">Flag indicates that argument is setup with default value</param>
        /// <param name="name">Argument name</param>
        /// <param name="value">Argument current value</param>
        public CommandLineArgumentInfo(PropertyInfo propertyInfo, bool setupByDefault, string name, object value)
        {
            PropertyInfo = propertyInfo;
            SetupByDefault = setupByDefault;
            Name = name;
            Value = value;
        }

        private PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Flag indicates that argument is setup with default value
        /// </summary>
        public bool SetupByDefault { get; }

        /// <summary>
        /// Argument name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Argument current value
        /// </summary>
        public object Value { get; }
    }
}