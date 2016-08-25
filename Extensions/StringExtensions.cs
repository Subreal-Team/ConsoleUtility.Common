using System;
using System.Linq;

namespace SubrealTeam.Common.Extensions
{
	/// <summary>
	/// Методы расширений для строк
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Форматирует строку с параметрами, используя string.Format.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string Args(this String target, params object[] args)
		{
			return string.Format(target, args);
		}

		#region IsEmpty, IsNotEmpty

		/// <summary>
		/// Указывает, является ли заданная строка значением null, пустой строкой или строкой, состоящей только из пробельных символов.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsEmpty(this string value)
		{
			return String.IsNullOrWhiteSpace(value);
		}

		/// <summary>
		/// Указывает что заданная строка НЕ является значением null, пустой строкой или строкой, состоящей только из пробельных символов.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNotEmpty(this string value)
		{
			return !value.IsEmpty();
		}

		#endregion

		#region NotNull, NotEmpty

		/// <summary>
		/// Возвращает пустую строку если заданная строка является значением null, в противном случае саму строку
		/// </summary>
		/// <param name="target">Строка</param>
		/// <returns></returns>
		public static string NotNull(this string target)
		{
			return ReferenceEquals(target, null) ? String.Empty : target;
		}

		/// <summary>
		///  Возвращает значение по умолчанию если заданная строка пустая, в противном случае саму строку
		/// </summary>
		/// <param name = "value">The string to check.</param>
		/// <param name = "defaultValue">The default value.</param>
		/// <returns>Either the string or the default value.</returns>
		public static string NotEmpty(this string value, string defaultValue)
		{
			return value.IsNotEmpty() ? value : defaultValue;
		}

		#endregion

		#region ContainsAny, ContainsAll

		/// <summary>
		/// Проверяет вхождение любой из указаных строк в данном экземпляре
		/// </summary>
		/// <param name="inputValue"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static bool ContainsAny(this string inputValue, params string[] values)
		{
			return inputValue.IsNotEmpty() && values.Any(p => inputValue.IndexOf(p, StringComparison.Ordinal) != -1);
		}

		/// <summary>
		/// Проверяет вхождение всех указаных строк в данном экземпляре
		/// </summary>
		/// <param name="inputValue"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static bool ContainsAll(this string inputValue, params string[] values)
		{
			return inputValue.IsNotEmpty() && values.All(p => inputValue.IndexOf(p, StringComparison.Ordinal) != -1);
		}

		#endregion

		#region LikeAny, Like

		/// <summary>
		/// Проверяет соответствие строки любому из указанных шаблонов
		/// </summary>
		/// <param name="value">Строка<see cref="System.String"/> object</param>
		/// <param name="patterns">Шаблоны</param>
		/// <returns>Признак соответствия</returns>
		public static bool LikeAny(this string value, params string[] patterns)
		{
			return patterns.Any(value.Like);
		}

		/// <summary>
		/// Проверяет соответствие строки указанному шаблону
		/// </summary>
		/// <param name="value">Строка</param>
		/// <param name="pattern">Шаблон</param>
		/// <returns>Признак соответствия</returns>
		public static bool Like(this string value, string pattern)
		{
			if (value == pattern || pattern == "*") return true;

			if (pattern.StartsWith("*"))
			{
				return value.Where((t, index) => value.Substring(index).Like(pattern.Substring(1))).Any();
			}
			if (pattern[0] == value[0])
			{
				return value.Substring(1).Like(pattern.Substring(1));
			}
			return false;
		}

		#endregion

	}
}
