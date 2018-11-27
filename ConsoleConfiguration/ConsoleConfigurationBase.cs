using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SubrealTeam.Common.Logging;

namespace SubrealTeam.Common.ConsoleConfiguration
{
	/// <summary>
	/// Базовый класс конфигурации консольного приложения
	/// </summary>
	public abstract class ConsoleConfigurationBase
	{
        protected string[] _arguments;

	    /// <summary>
	    /// Аргументы командной строки
	    /// </summary>
	    public string[] Arguments => _arguments;

	    /// <summary>
	    /// Установить аргументы
	    /// </summary>
	    protected virtual void SetArguments()
		{
			_arguments = Environment.GetCommandLineArgs();
		}

		/// <summary>
		/// Флаг - параметры не указаны
		/// </summary>
		public bool NoParameters => Arguments.Length == 1;

	    /// <summary>
		/// Флаг ошибки считвания параметров
		/// </summary>
		public bool NotValidParameters => NotValidParamtersMessages.Any();

	    /// <summary>
		/// Сообщения об ошибке считывания параметров
		/// </summary>
		public List<string> NotValidParamtersMessages { get; }

        /// <summary>
        /// Кешь свойств класса с атрибутом командной строки
        /// </summary>
	    private AttributedPropertyInfo[] _attributedProps;

        /// <summary>
        /// Конструктор конфигурации консольного приложения
        /// </summary>
        protected ConsoleConfigurationBase()
		{
			SetArguments();

			NotValidParamtersMessages = new List<string>();

			// по всем публичным свойствам класса
			var typeInfo = this.GetType();
			var publicProps = typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// взять свойства с атрибутом CommandLineArgument
			_attributedProps = publicProps.Select(x => new {property = x, customAttributes = x.GetCustomAttributes()})
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
			if (cmdAttr.DefaultValue == null && propertyInfo.PropertyType.Name != "String")
				cmdAttr.DefaultValue = Activator.CreateInstance(propertyInfo.PropertyType);

			var cmdValue = Arguments.FirstOrDefault(x => x.ToUpper().StartsWith(cmdAttr.Name.ToUpper()));
			// аргумент не указан, взять значение по умолчанию
			if (String.IsNullOrWhiteSpace(cmdValue))
			{
				propertyInfo.SetValue(this, Convert.ChangeType(cmdAttr.DefaultValue, propertyInfo.PropertyType));
				return;
			}

			var match = Regex.Match(cmdAttr.ParseTemplate, "{name}(.*){value}");
			// неверно задан шаблон атрибута
			if (!match.Success || match.Groups.Count <= 0)
			{
				var errorMessage =
				    $"Неверно задан шаблон атрибута {cmdAttr.ParseTemplate}, формат: {{имя}}{{разделитель}}{{значение}}";
				NotValidParamtersMessages.Add(errorMessage);
				Logger.Error(errorMessage);
				return;
			}

			var splitter = match.Groups[1].Value;
			var value = Regex.Split(cmdValue, splitter);

			// указано значение атрибута, пробуем сконвертировать
			object convertedValue;
			try
			{
				if ((value.Length == 2) && !String.IsNullOrWhiteSpace(value[1]))
				{
					if (propertyInfo.PropertyType.Name == "Decimal")
					{
						value[1] = value[1].Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator)
							.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
					}
					convertedValue = Convert.ChangeType(value[1], propertyInfo.PropertyType);
				}
				else convertedValue = Convert.ChangeType(cmdAttr.DefaultValue, propertyInfo.PropertyType);
			}
			catch (FormatException e)
			{
				var errorMessage =
				    $"Ошибка конвертирования параметра \"{cmdValue}\", тип аргумента: \"{propertyInfo.PropertyType.Name}\"";
				NotValidParamtersMessages.Add(errorMessage);
				Logger.Error(errorMessage);
				return;
			}

			propertyInfo.SetValue(this, convertedValue);
		}

	    /// <summary>
	    /// Печать описания параметров с выводом в консоль
	    /// </summary>
	    /// <returns>флаг вывода в консоль</returns>
	    public string PrintHelp(bool printToConsole = true)
	    {
	        var helpMessage = String.Join(Environment.NewLine,
	            _attributedProps.SelectMany(a => a.Attributes.Select(x => $"{x.Name} - {x.Description}").ToArray()));
	        
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
