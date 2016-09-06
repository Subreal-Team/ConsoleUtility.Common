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
		protected string[] Arguments
		{
			get { return _arguments; }
		}

		public virtual void SetArguments()
		{
			_arguments = Environment.GetCommandLineArgs();
		}


		/// <summary>
		/// Флаг - параметры не указаны
		/// </summary>
		public bool NoParameters { get { return Arguments.Length == 1; } }

		/// <summary>
		/// Флаг ошибки считвания параметров
		/// </summary>
		public bool NotValidParameters { get { return NotValidParamtersMessages.Any(); } }

		/// <summary>
		/// Сообщения об ошибке считывания параметров
		/// </summary>
		public List<string> NotValidParamtersMessages { get; private set; }

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
			var attributedProps = publicProps.Select(x => new {property = x, customAttributes = x.GetCustomAttributes()})
				.Where(x => x.customAttributes.Any(y => y.GetType() == typeof(CommandLineArgumentAttribute)))
				.ToList();

			foreach (var prop in attributedProps)
			{
				foreach (var attr in prop.customAttributes)
				{
					SetPropertyValue((CommandLineArgumentAttribute) attr, prop.property);
				}
			}
		}

		private void SetPropertyValue(CommandLineArgumentAttribute cmdAttr, PropertyInfo propertyInfo)
		{		
			if (cmdAttr.DefaultValue == null)
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
				var errorMessage = String.Format("Неверно задан шаблон атрибута {0}, формат: {{имя}}{{разделитель}}{{значение}}",
					cmdAttr.ParseTemplate);
				NotValidParamtersMessages.Add(errorMessage);
				Logger.Instance.Error(errorMessage);
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
				var errorMessage = String.Format("Ошибка конвертирования параметра \"{0}\", тип аргумента: \"{1}\"", cmdValue,
					propertyInfo.PropertyType.Name);
				NotValidParamtersMessages.Add(errorMessage);
				Logger.Instance.Error(errorMessage);
				return;
			}

			propertyInfo.SetValue(this, convertedValue);
		}

	}
}
