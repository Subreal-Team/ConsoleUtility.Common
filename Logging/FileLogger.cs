using System;
using System.IO;
using System.Reflection;

namespace SubrealTeam.Common.Logging
{
	/// <summary>
	/// Реализация файлового логгера
	/// </summary>
	public class FileLogger : ILogger
	{
        private readonly string _fileName;

        private readonly string _filePath;

        private readonly string _fileFullName;

        public FileLogger(string fileName = null, string filePath = null)
        {
            var assembly = Assembly.GetEntryAssembly();
            _fileName = fileName ?? $"{assembly.GetName().Name} {DateTime.Now:yyyy-MM-dd}.log";
            _filePath = filePath ?? assembly.Location;
            _fileFullName = Path.Combine(_filePath, _fileName);

            try
            {
                FileStream fileStream;
                if (!File.Exists(_fileFullName))
                {
                    fileStream = File.Create(_fileFullName);
                }
                else
                {
                    fileStream = File.OpenWrite(_fileFullName);
                }

                fileStream.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AppendLog(string message)
        {
            File.AppendAllText(_fileFullName, message);
        }

        private string GetFormattedDateTime()
        {
            return $"{DateTime.Now:O}";
        }

		public ILogger GetNamedLogger(string loggerName)
		{
            return new FileLogger();
        }

		public void Debug(string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()} [DEBUG]: {string.Format(format, args)}");
        }

		public void Error(string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()} [ERROR]: {string.Format(format, args)}");
        }

		public void Error(Exception exception, string message = null)
		{
            AppendLog($"{GetFormattedDateTime()} [ERROR]: {message}. {exception.Message}");
        }

		public void Error(Exception exception, string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()} [ERROR]: {string.Format(format, args)}. {exception.Message}");
        }

		public void Fatal(string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()} [FATAL]: {string.Format(format, args)}");
        }

		public void Fatal(Exception exception, string message = null)
		{
            AppendLog($"{GetFormattedDateTime()} [FATAL]: {message}. {exception.Message}");
        }

		public void Fatal(Exception exception, string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()} [FATAL]: {string.Format(format, args)}. {exception.Message}");
        }

		public void Info(string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()}: {string.Format(format, args)}");
        }

		public void Warn(string format, params object[] args)
		{
            AppendLog($"{GetFormattedDateTime()} [WARN]: {string.Format(format, args)}");
		}
	}
}
