using System;

namespace SubrealTeam.Windows.Common.Logging
{
	/// <summary>
	/// Реализация консольного логгера
	/// </summary>
	public class ConsoleLogger : ILogger
	{
		public ILogger GetNamedLogger(string loggerName)
		{
			throw new NotImplementedException();
		}

		public void Debug(string format, params object[] args)
		{
			Console.WriteLine("{0} [DEBUG]: {1}", DateTime.Now.ToString("O"), String.Format(format, args));
		}

		public void Error(string format, params object[] args)
		{
			Console.WriteLine("{0} [ERROR]: {1}", DateTime.Now.ToString("O"), String.Format(format, args));
		}

		public void Error(Exception exception, string message = null)
		{
			throw new NotImplementedException();
		}

		public void Error(Exception exception, string format, params object[] args)
		{
			throw new NotImplementedException();
		}

		public void Fatal(string format, params object[] args)
		{
			Console.WriteLine("{0} [FATAL]: {1}", DateTime.Now.ToString("O"), String.Format(format, args));
		}

		public void Fatal(Exception exception, string message = null)
		{
			throw new NotImplementedException();
		}

		public void Fatal(Exception exception, string format, params object[] args)
		{
			throw new NotImplementedException();
		}

		public void Info(string format, params object[] args)
		{
			Console.WriteLine("{0} [INFO]: {1}", DateTime.Now.ToString("O"), String.Format(format, args));
		}

		public void Warn(string format, params object[] args)
		{
			Console.WriteLine("{0} [WARN]: {1}", DateTime.Now.ToString("O"), String.Format(format, args));
		}
	}
}
