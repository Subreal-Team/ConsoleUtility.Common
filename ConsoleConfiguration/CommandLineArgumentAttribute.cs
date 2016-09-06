using System;

namespace SubrealTeam.Common.ConsoleConfiguration
{
	/// <summary>
	/// Атрибут определяющий конфигурационный параметр командной строки
	/// </summary>
	public class CommandLineArgumentAttribute : Attribute
	{
		/// <summary>
		/// Создать аргумент конфигурации командной строки
		/// </summary>
		/// <param name="name">Наименование параметра командной строки</param>
		/// <param name="parseTemplate">Шаблон значения атрибута. default: {name}={value}</param>
		/// <param name="defaultValue">Значение по умолчанию. default: ""</param>
		/// <param name="description">Описание параметра.</param>
		public CommandLineArgumentAttribute(string name, string parseTemplate = "{name}={value}", object defaultValue = null, string description = "")
		{
			Name = name;
			ParseTemplate = parseTemplate;
			DefaultValue = defaultValue;
			Description = description;
		}

		/// <summary>
		/// Наименование параметра командной строки
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Шаблон значения атрибута
		/// {name}={value} (по умолчанию).
		///		Например: /{name}:{value}
		/// </summary>
		public string ParseTemplate { get; private set; }

		/// <summary>
		/// Значение по умолчанию
		/// </summary>
		public object DefaultValue { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }
	}
}
