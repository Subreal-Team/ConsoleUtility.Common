namespace SubRealTeam.ConsoleUtility.Common.Logging
{
    /// <summary>
    /// Console Logger implementation
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <inheritdoc />
        public ILogger GetNamedLogger(string loggerName)
        {
            return new ConsoleLogger();
        }

        /// <summary>
        /// Log Level
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <inheritdoc />
        public void Debug(string format, params object[] args)
        {
            if (LogLevel > LogLevel.Debug) return;
            Console.WriteLine($"{DateTime.Now:O} [DEBUG]: {string.Format(format, args)}");
        }

        /// <inheritdoc />
        public void Error(string format, params object[] args)
        {
            if (LogLevel > LogLevel.Error) return;
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [ERROR]: {string.Format(format, args)}");
            Console.ForegroundColor = color;
        }

        /// <inheritdoc />
        public void Error(Exception exception, string? message = null)
        {
            if (LogLevel > LogLevel.Error) return;
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [ERROR]: {message}. {exception.Message}");
            Console.ForegroundColor = color;
        }

        /// <inheritdoc />
        public void Error(Exception exception, string format, params object[] args)
        {
            if (LogLevel > LogLevel.Error) return;
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [ERROR]: {string.Format(format, args)}. {exception.Message}");
            Console.ForegroundColor = color;
        }

        /// <inheritdoc />
        public void Fatal(string format, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [FATAL]: {string.Format(format, args)}");
            Console.ForegroundColor = color;
        }

        /// <inheritdoc />
        public void Fatal(Exception exception, string? message = null)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [FATAL]: {message}. {exception.Message}");
            Console.ForegroundColor = color;
        }

        /// <inheritdoc />
        public void Fatal(Exception exception, string format, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now:O} [FATAL]: {string.Format(format, args)}. {exception.Message}");
            Console.ForegroundColor = color;
        }

        /// <inheritdoc />
        public void Info(string format, params object[] args)
        {
            if (LogLevel > LogLevel.Info) return;
            Console.WriteLine($"{DateTime.Now:O}: {string.Format(format, args)}");
        }

        /// <inheritdoc />
        public void Warn(string format, params object[] args)
        {
            if (LogLevel > LogLevel.Warn) return;
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now:O} [WARN]: {string.Format(format, args)}");
            Console.ForegroundColor = color;
        }
    }
}
