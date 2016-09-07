using System;

namespace SubrealTeam.Common.Logging
{
	/// <summary>
	/// Реализация консольного логгера
	/// </summary>
	public class ConsoleLogger : ILogger
	{
		public ILogger GetNamedLogger(string loggerName)
		{
            return new ConsoleLogger();
        }

		public void Debug(string format, params object[] args)
		{
            Console.WriteLine($"{DateTime.Now:O} [DEBUG]: {String.Format(format, args)}");
        }

		public void Error(string format, params object[] args)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [ERROR]: {String.Format(format, args)}");
            Console.ForegroundColor = color;
        }

		public void Error(Exception exception, string message = null)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [ERROR]: {message}. {exception.Message}");
            Console.ForegroundColor = color;
        }

		public void Error(Exception exception, string format, params object[] args)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [ERROR]: {String.Format(format, args)}. {exception.Message}");
            Console.ForegroundColor = color;
        }

		public void Fatal(string format, params object[] args)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [FATAL]: {String.Format(format, args)}");
            Console.ForegroundColor = color;
        }

		public void Fatal(Exception exception, string message = null)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [FATAL]: {message}. {exception.Message}");
            Console.ForegroundColor = color;
        }

		public void Fatal(Exception exception, string format, params object[] args)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [FATAL]: {String.Format(format, args)}. {exception.Message}");
            Console.ForegroundColor = color;
        }

		public void Info(string format, params object[] args)
		{
            Console.WriteLine($"{DateTime.Now:O}: {String.Format(format, args)}");
        }

		public void Warn(string format, params object[] args)
		{
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now:O} [WARN]: {String.Format(format, args)}");
            Console.ForegroundColor = color;
		}
	}
}
