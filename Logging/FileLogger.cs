using System;
using System.IO;
using System.Reflection;

namespace SubRealTeam.ConsoleUtility.Common.Logging
{
    /// <summary>
    /// File Logger implementation
    /// </summary>
    public class FileLogger : ILogger
    {

        private readonly string _fileFullName;

        /// <summary>
        /// File logger constructor
        /// </summary>
        /// <param name="fileName">File name for log (default is current date in format YYYY-MM-DD.log)</param>
        /// <param name="filePath">File Path for log (default is current directory)</param>
        public FileLogger(string fileName = null, string filePath = null)
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                fileName = fileName ?? $"{assembly.GetName().Name} {DateTime.Now:yyyy-MM-dd}.log";
                filePath = filePath ?? Path.GetDirectoryName(assembly.Location);
                _fileFullName = Path.Combine(filePath, fileName);
            }
            else
            {
                _fileFullName = Path.Combine(Directory.GetCurrentDirectory(), $"Log {DateTime.Now:yyyy-MM-dd}.log");
            }

            try
            {
                var fileStream = !File.Exists(_fileFullName) 
                    ? File.Create(_fileFullName) 
                    : File.OpenWrite(_fileFullName);

                fileStream.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AppendLog(string message)
        {
            File.AppendAllText(_fileFullName, Environment.NewLine + message);
        }

        private string GetFormattedDateTime()
        {
            return $"{DateTime.Now:O}";
        }

        /// <inheritdoc />
        public ILogger GetNamedLogger(string loggerName)
        {
            return new FileLogger();
        }

        /// <inheritdoc />
        public void Debug(string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()} [DEBUG]: {string.Format(format, args)}");
        }

        /// <inheritdoc />
        public void Error(string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()} [ERROR]: {string.Format(format, args)}");
        }

        /// <inheritdoc />
        public void Error(Exception exception, string message = null)
        {
            AppendLog($"{GetFormattedDateTime()} [ERROR]: {message}. {exception.Message}");
        }

        /// <inheritdoc />
        public void Error(Exception exception, string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()} [ERROR]: {string.Format(format, args)}. {exception.Message}");
        }

        /// <inheritdoc />
        public void Fatal(string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()} [FATAL]: {string.Format(format, args)}");
        }

        /// <inheritdoc />
        public void Fatal(Exception exception, string message = null)
        {
            AppendLog($"{GetFormattedDateTime()} [FATAL]: {message}. {exception.Message}");
        }

        /// <inheritdoc />
        public void Fatal(Exception exception, string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()} [FATAL]: {string.Format(format, args)}. {exception.Message}");
        }

        /// <inheritdoc />
        public void Info(string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()}: {string.Format(format, args)}");
        }

        /// <inheritdoc />
        public void Warn(string format, params object[] args)
        {
            AppendLog($"{GetFormattedDateTime()} [WARN]: {string.Format(format, args)}");
        }
    }
}
