using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Аргументы командной строки
        /// </summary>
		protected string[] _arguments = Environment.GetCommandLineArgs();

        /// <summary>
        /// Конструктор конфигурации консольного приложения
        /// </summary>
        protected ConsoleConfigurationBase()
		{
			NotValidParameters = false;
			NotValidParamtersMessages = new List<string>();

			// по всем публичным свойствам класса
			var typeInfo = this.GetType();
			var publicProps = typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// взять свойства с атрибутом CommandLineArgument
			var attributedProps = publicProps.Select(x => new { property = x, customAttributes = x.GetCustomAttributes() })
				.Where(x => x.customAttributes.Any(y => y.GetType() == typeof(CommandLineArgumentAttribute)))
				.ToList();

			foreach (var prop in attributedProps)
			{
				foreach (var attr in prop.customAttributes)
				{
					var cmdAttr = (CommandLineArgumentAttribute)attr;
					var cmdValue = _arguments.FirstOrDefault(x => x.ToUpper().Contains(cmdAttr.Name.ToUpper()));
					// аргумент не указан, взять значение по умолчанию
					if (String.IsNullOrWhiteSpace(cmdValue))
					{
						prop.property.SetValue(this, cmdAttr.DefaultValue);
						continue;
					}

					var match = Regex.Match(cmdAttr.ParseTemplate, "{name}(.*){value}");
					// неверно задан параметр
					if (!match.Success || match.Groups.Count <= 0)
					{
						NotValidParameters = true;
						var errorMessage = String.Format("Неверно задан параметр {0}, формат: {1}", cmdAttr.Name, cmdAttr.ParseTemplate);
						NotValidParamtersMessages.Add(errorMessage);
						Logger.Instance.Error(errorMessage);
						continue;
					}

					var splitter = match.Groups[1].Value;
					var value = Regex.Split(cmdValue, splitter);
					prop.property.SetValue(this,
						(value.Length == 2) && !String.IsNullOrWhiteSpace(value[1])
							? value[1]
							: cmdAttr.DefaultValue);

				}
			}
		}

		/// <summary>
		/// Флаг - параметры не указаны
		/// </summary>
		public bool NoParameters { get { return _arguments.Length == 1; } }

		/// <summary>
		/// Флаг ошибки считвания параметров
		/// </summary>
		public bool NotValidParameters { get; private set; }

		/// <summary>
		/// Сообщения об ошибке считывания параметров
		/// </summary>
		public List<string> NotValidParamtersMessages { get; private set; }
	}
}
