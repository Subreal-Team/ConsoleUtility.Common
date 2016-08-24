using System;

namespace SubrealTeam.Windows.Common.Logging
{
	/// <summary>
	/// Синглтон класс логирования
	/// </summary>
	public static class Logger
	{
		private static ILogger _instance;

		public static ILogger Instance
		{
			get { return _instance ?? (_instance = new ConsoleLogger()); }
		}

		/// <summary>  Получение именнованного логгера </summary>
		public static ILogger GetNamedLogger(String loggerName)
		{
			return Instance.GetNamedLogger(loggerName);
		}

		/// <summary> Логирование (уровень DEBUG) форматированной строки </summary>
		public static void Debug(string format, params object[] args)
		{
			Instance.Debug(format, args);
		}

		/// <summary> Логирование (уровень ERROR) форматированной строки </summary>
		public static void Error(string format, params object[] args)
		{
			Instance.Error(format, args);
		}

		/// <summary>  Логирование (уровень ERROR) исключения с сообщением, если сообщение не указано - логирование исключения </summary>
		public static void Error(Exception exception, string message = null)
		{
			Instance.Error(exception, message);
		}

		/// <summary> Логирование (уровень ERROR) исключения с сообщением (форматированная строка) </summary>
		public static void Error(Exception exception, string format, params object[] args)
		{
			Instance.Error(exception, format, args);
		}

		/// <summary> Логирование (уровень FATAL) форматированной строки </summary>
		public static void Fatal(string format, params object[] args)
		{
			Instance.Fatal(format, args);
		}

		/// <summary> Логирование (уровень FATAL) исключения с сообщением, если сообщение не указано - логирование исключения </summary>
		public static void Fatal(Exception exception, string message = null)
		{
			Instance.Fatal(exception, message);
		}

		/// <summary> Логирование (уровень FATAL) исключения с сообщением (форматированная строка) </summary>
		public static void Fatal(Exception exception, string format, params object[] args)
		{
			Instance.Fatal(exception, format, args);
		}

		/// <summary> Логирование (уровень INFO) форматированной строки </summary>
		public static void Info(string format, params object[] args)
		{
			Instance.Info(format, args);
		}

		/// <summary> Логирование (уровень WARN) форматированной строки </summary>
		public static void Warn(string format, params object[] args)
		{
			Instance.Warn(format, args);
		}
	}
}
