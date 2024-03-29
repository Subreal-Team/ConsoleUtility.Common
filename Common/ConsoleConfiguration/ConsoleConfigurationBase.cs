﻿using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using SubRealTeam.ConsoleUtility.Common.Logging;

namespace SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration
{
    /// <summary>
    /// Base class configuration of the console application
    /// </summary>
    public abstract class ConsoleConfigurationBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string[] _arguments;

        /// <summary>
        /// Flag - no parameters specified
        /// </summary>
        public bool NoParameters => _arguments.Length == 0;

        /// <summary>
        /// Parameter read error flag
        /// </summary>
        public bool NotValidParameters => NotValidParametersMessages.Any();

        /// <summary>
        /// Parameters Read Error Messages
        /// </summary>
        public List<string> NotValidParametersMessages { get; }

        /// <summary>
        /// Cache class properties with command line attribute
        /// </summary>
        private readonly CommandLinePropertyInfo[] _attributedProps;

        /// <summary>
        /// Console Configuration constructor
        /// </summary>
        protected ConsoleConfigurationBase() : this(null)
        {
        }

        /// <summary>
        /// Console Configuration constructor
        /// </summary>
        /// <param name="arguments">Array of string to get command line arguments not from System.Environment</param>
        protected ConsoleConfigurationBase(string[]? arguments = null)
        {
            _arguments = arguments ?? Environment.GetCommandLineArgs().Skip(1).ToArray();

            NotValidParametersMessages = new List<string>();

            var typeInfo = this.GetType();
            var publicProps = typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            _attributedProps = publicProps.Select(x => new {property = x, customAttributes = x.GetCustomAttributes(true)})
               .Where(x => x.customAttributes.Any(y => y.GetType() == typeof(CommandLineArgumentAttribute)))
               .Select(x => new CommandLinePropertyInfo(
                   x.property, 
                   (x.customAttributes.First(y => y.GetType() == typeof(CommandLineArgumentAttribute)) as CommandLineArgumentAttribute)!, 
                   null, 
                   false, 
                   x.customAttributes.FirstOrDefault(y => y.GetType() == typeof(RequiredArgumentAttribute)) != null))
               .ToArray();

            foreach (var prop in _attributedProps)
            {
                SetPropertyValue(prop);
            }
        }

        private void SetPropertyValue(CommandLinePropertyInfo commandLinePropertyInfo)
        {
            var cmdAttr = commandLinePropertyInfo.Attribute;
            var propertyInfo = commandLinePropertyInfo.PropertyInfo;

            var cmdValue = _arguments.FirstOrDefault(x => x.ToUpper().StartsWith(cmdAttr.Name.ToUpper()));
            if (string.IsNullOrWhiteSpace(cmdValue))
            {
                if (!cmdAttr.DefaultValueIsSetup)
                {
                    if (commandLinePropertyInfo.Required)
                    {
                        var errorMessage = $"Attribute '{cmdAttr.Name}' requires a value";
                        NotValidParametersMessages.Add(errorMessage);
                        Logger.Error(errorMessage);
                    }
                    
                    return;
                }

                if (propertyInfo.CanWrite)
                {
                    var attrValue = Convert.ChangeType(cmdAttr.DefaultValue, propertyInfo.PropertyType);
                    commandLinePropertyInfo.Value = attrValue;
                    commandLinePropertyInfo.SetupByDefault = true;
                    propertyInfo.SetValue(this, attrValue, null);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"The {propertyInfo.Name} property does not have a public setter.");
                }

                return;
            }

            var match = Regex.Match(cmdAttr.ParseTemplate, "{name}(.*){value}");
            if (!match.Success || match.Groups.Count <= 0)
            {
                var errorMessage =
                    $"Invalid attribute template {cmdAttr.ParseTemplate}, format: {{name}}{{delimiter}}{{value}}";
                NotValidParametersMessages.Add(errorMessage);
                Logger.Error(errorMessage);
                return;
            }

            var splitter = match.Groups[1].Value;
            var value = Regex.Split(cmdValue, splitter);

            object? convertedValue;
            try
            {
                if (value.Length == 2 && !string.IsNullOrWhiteSpace(value[1]))
                {
                    if (propertyInfo.PropertyType.Name == nameof(Decimal))
                    {
                        value[1] = value[1].Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator)
                            .Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                    }

                    if (propertyInfo.PropertyType.Name == nameof(Boolean) && (value[1] == "1" || value[1] == "0"))
                    {
                        convertedValue = value[1] == "1";

                        propertyInfo.SetValue(this, convertedValue, null);
                        return;
                    }

                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        convertedValue = Enum.Parse(propertyInfo.PropertyType, value[1]);
                        propertyInfo.SetValue(this, convertedValue, null);
                        return;
                    }

                    convertedValue = Convert.ChangeType(value[1], propertyInfo.PropertyType);
                }
                else
                {
                    convertedValue = Convert.ChangeType(cmdAttr.DefaultValue, propertyInfo.PropertyType);
                }
            }
            catch (FormatException e)
            {
                var errorMessage =
                    $"Error converting parameter \"{cmdValue}\", argument type: \"{propertyInfo.PropertyType.Name}\", Exception message: {e.Message}.";
                NotValidParametersMessages.Add(errorMessage);
                Logger.Error(errorMessage);
                return;
            }

            commandLinePropertyInfo.Value = convertedValue;
            propertyInfo.SetValue(this, convertedValue, null);
        }

        /// <summary>
        /// Printing parameter descriptions with console output
        /// </summary>
        /// <returns>Console output flag</returns>
        public string PrintHelp(bool printToConsole = true)
        {
            string HelpParameterDescription(CommandLineArgumentAttribute x) => 
                $"{x.Name} - {x.Description}{(x.DefaultValueIsSetup ? $" (default value is '{x.DefaultValue}')" : string.Empty)}";

            var helpMessage = string.Join(Environment.NewLine,
                _attributedProps.Select(a => HelpParameterDescription(a.Attribute)));

            if (printToConsole) Console.WriteLine(helpMessage);
            return helpMessage;
        }

        /// <summary>
        /// Get command line argument information
        /// </summary>
        /// <param name="name">Argument name</param>
        public CommandLineArgumentInfo? GetCommandLineArgumentInfo(string name)
        {
            return _attributedProps.Where(
                    x => string.Equals(x.Attribute.Name, name, StringComparison.OrdinalIgnoreCase))
                .Select(
                    x => new CommandLineArgumentInfo(
                        x.PropertyInfo,
                        x.SetupByDefault,
                        x.Attribute.Name,
                        x.Value))
                .FirstOrDefault();
        }

    }

    internal class CommandLinePropertyInfo
    {
        public CommandLinePropertyInfo(PropertyInfo propertyInfo, CommandLineArgumentAttribute attribute, object? value, bool setupByDefault, bool required)
        {
            PropertyInfo = propertyInfo;
            Attribute = attribute;
            Value = value;
            SetupByDefault = setupByDefault;
            Required = required;
        }

        public PropertyInfo PropertyInfo { get; set; }

        public CommandLineArgumentAttribute Attribute { get; set; }

        public object? Value { get; set; }

        public bool SetupByDefault { get; set; }
        
        public bool Required { get; set; }
    }
}
