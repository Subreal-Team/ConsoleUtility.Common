﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        protected string[] Arguments;

        /// <summary>
        /// Flag - no parameters specified
        /// </summary>
        public bool NoParameters => Arguments.Length == 0;

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
        private readonly AttributedPropertyInfo[] _attributedProps;

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
        protected ConsoleConfigurationBase(string[] arguments = null)
        {
            Arguments = arguments ?? Environment.GetCommandLineArgs().Skip(1).ToArray();

            NotValidParametersMessages = new List<string>();

            var typeInfo = this.GetType();
            var publicProps = typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            _attributedProps = publicProps.Select(x => new {property = x, customAttributes = x.GetCustomAttributes(true)})
               .Where(x => x.customAttributes.Any(y => y.GetType() == typeof(CommandLineArgumentAttribute)))
               .Select(x => new AttributedPropertyInfo
                {
                    PropertyInfo = x.property,
                    Attributes = x.customAttributes.Select(a => a as CommandLineArgumentAttribute).ToArray()
                })
               .ToArray();

            foreach (var prop in _attributedProps)
            {
                foreach (var attr in prop.Attributes)
                {
                    SetPropertyValue(attr, prop.PropertyInfo);
                }
            }
        }

        private void SetPropertyValue(CommandLineArgumentAttribute cmdAttr, PropertyInfo propertyInfo)
        {
            var cmdValue = Arguments.FirstOrDefault(x => x.ToUpper().StartsWith(cmdAttr.Name.ToUpper()));
            if (string.IsNullOrWhiteSpace(cmdValue))
            {
                if (!cmdAttr.DefaultValueIsSetup)
                {
                    return;
                }

                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(this, Convert.ChangeType(cmdAttr.DefaultValue, propertyInfo.PropertyType),
                        null);
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

            object convertedValue;
            try
            {
                if ((value.Length == 2) && !string.IsNullOrWhiteSpace(value[1]))
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

                    convertedValue = Convert.ChangeType(value[1], propertyInfo.PropertyType);
                }
                else convertedValue = Convert.ChangeType(cmdAttr.DefaultValue, propertyInfo.PropertyType);
            }
            catch (FormatException e)
            {
                var errorMessage =
                    $"Error converting parameter \"{cmdValue}\", argument type: \"{propertyInfo.PropertyType.Name}\", Exception message: {e.Message}.";
                NotValidParametersMessages.Add(errorMessage);
                Logger.Error(errorMessage);
                return;
            }

            propertyInfo.SetValue(this, convertedValue, null);
        }

        /// <summary>
        /// Printing parameter descriptions with console output
        /// </summary>
        /// <returns>Console output flag</returns>
        public string PrintHelp(bool printToConsole = true)
        {
            Func<CommandLineArgumentAttribute, string> helpParameterDescription = x =>
                $"{x.Name} - {x.Description}{(x.DefaultValueIsSetup ? $" (default value is '{x.DefaultValue}')" : string.Empty)}";

            var helpMessage = string.Join(Environment.NewLine,
                _attributedProps.SelectMany(a => a.Attributes.Select(helpParameterDescription).ToArray()));

            if (printToConsole) Console.WriteLine(helpMessage);
            return helpMessage;
        }

    }

    internal class AttributedPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; set; }

        public CommandLineArgumentAttribute[] Attributes { get; set; }
    }
}
