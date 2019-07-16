using System;
using System.IO;
using System.Reflection;

namespace SubrealTeam.Common.Logging
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
        /// <param name="fileName">File name for log</param>
        /// <param name="filePath">File Path for log</param>
        public FileLogger(string fileName = null, string filePath = null)
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                fileName = fileName ?? $"{assembly.GetName().Name} {DateTime.Now:yyyy-MM-dd}.log";
                filePath = filePath ?? assembly.Location;
                _fileFullName = Path.Combine(filePath, fileName);
            }
            else
            {
                _fileFullName = Path.Combine(Directory.GetCurrentDirectory(), $"Log {DateTime.Now:yyyy-MM-dd}.log");
            }

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
