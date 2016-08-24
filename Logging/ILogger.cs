using System;

namespace SubrealTeam.Windows.Common.Logging
{
	/// <summary> Интерфейс для реализации логирования </summary>
	public interface ILogger
	{
		/// <summary> Получение именованного логера </summary>
		ILogger GetNamedLogger(string loggerName);

		/// <summary> Логирование (уровень DEBUG) форматированной строки </summary>
		void Debug(string format, params object[] args);

		/// <summary> Логирование (уровень ERROR) форматированной строки </summary>
		void Error(string format, params object[] args);

		/// <summary>  Логирование (уровень ERROR) исключения с сообщением, если сообщение не указано - логирование исключения </summary>
		void Error(Exception exception, string message = null);

		/// <summary> Логирование (уровень ERROR) исключения с сообщением (форматированная строка) </summary>
		void Error(Exception exception, string format, params object[] args);

		/// <summary> Логирование (уровень FATAL) форматированной строки </summary>
		void Fatal(string format, params object[] args);

		/// <summary> Логирование (уровень FATAL) исключения с сообщением, если сообщение не указано - логирование исключения </summary>
		void Fatal(Exception exception, string message = null);

		/// <summary> Логирование (уровень FATAL) исключения с сообщением (форматированная строка) </summary>
		void Fatal(Exception exception, string format, params object[] args);

		/// <summary> Логирование (уровень INFO) форматированной строки </summary>
		void Info(string format, params object[] args);

		/// <summary> Логирование (уровень WARN) форматированной строки </summary>
		void Warn(string format, params object[] args);
	}
}
