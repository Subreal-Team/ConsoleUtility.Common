using System;
using System.Collections.Generic;
using System.Linq;

namespace SubrealTeam.Common.Logging
{
	/// <summary>
	/// Синглтон класс логирования
	/// </summary>
	public static class Logger
	{
		private static ILogger _instance;

		private static IList<ILogger> Instances { get; set; } = new List<ILogger>();

        public static void AddInstance(ILogger logger)
        {
            Guard.IsNotNull(logger, nameof(logger));

            if (!Instances.Any(x => x.GetType() == logger.GetType()))
            {
                Instances.Add(logger);
            }
        }

		///// <summary>  Получение именнованного логгера </summary>
		//public static ILogger GetNamedLogger(String loggerName)
		//{
		//	return Instance.GetNamedLogger(loggerName);
		//}

		/// <summary> Логирование (уровень DEBUG) форматированной строки </summary>
		public static void Debug(string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Debug(format, args);
            }
		}

		/// <summary> Логирование (уровень ERROR) форматированной строки </summary>
		public static void Error(string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Error(format, args);
            }
		}

		/// <summary>  Логирование (уровень ERROR) исключения с сообщением, если сообщение не указано - логирование исключения </summary>
		public static void Error(Exception exception, string message = null)
		{
            foreach (var instance in Instances)
            {
                instance.Error(exception, message);
            }
		}

		/// <summary> Логирование (уровень ERROR) исключения с сообщением (форматированная строка) </summary>
		public static void Error(Exception exception, string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Error(exception, format, args);
            }
		}

		/// <summary> Логирование (уровень FATAL) форматированной строки </summary>
		public static void Fatal(string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Fatal(format, args);
            }
		}

		/// <summary> Логирование (уровень FATAL) исключения с сообщением, если сообщение не указано - логирование исключения </summary>
		public static void Fatal(Exception exception, string message = null)
		{
            foreach (var instance in Instances)
            {
                instance.Fatal(exception, message);
            }
		}

		/// <summary> Логирование (уровень FATAL) исключения с сообщением (форматированная строка) </summary>
		public static void Fatal(Exception exception, string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Fatal(exception, format, args);
            }
		}

		/// <summary> Логирование (уровень INFO) форматированной строки </summary>
		public static void Info(string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Info(format, args);
            }
		}

		/// <summary> Логирование (уровень WARN) форматированной строки </summary>
		public static void Warn(string format, params object[] args)
		{
            foreach (var instance in Instances)
            {
                instance.Warn(format, args);
            }
		}
	}
}
