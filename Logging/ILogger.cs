using System;

namespace SubRealTeam.ConsoleUtility.Common.Logging
{
    /// <summary> Interface for implementing logging</summary>
    public interface ILogger
    {
        /// <summary> Getting the named logger </summary>
        ILogger GetNamedLogger(string loggerName);
        
        LogLevel LogLevel { get; set; }

        /// <summary> Logging (DEBUG level) formatted string </summary>
        void Debug(string format, params object[] args);

        /// <summary> Logging (ERROR level) of formatted string </summary>
        void Error(string format, params object[] args);

        /// <summary> Logging (ERROR level) exceptions with a message, if the message is not specified - logging an exception </summary>
        void Error(Exception exception, string message = null);

        /// <summary> Logging (ERROR level) exception with message (formatted string) </summary>
        void Error(Exception exception, string format, params object[] args);

        /// <summary> Logging (FATAL level) formatted string </summary>
        void Fatal(string format, params object[] args);

        /// <summary> Logging (FATAL level) exceptions with a message, if the message is not specified - exception logging </summary>
        void Fatal(Exception exception, string message = null);

        /// <summary> Logging (FATAL level) exceptions with message (formatted string) </summary>
        void Fatal(Exception exception, string format, params object[] args);

        /// <summary> Logging (INFO level) of formatted string </summary>
        void Info(string format, params object[] args);

        /// <summary> Logging (WARN level) formatted string </summary>
        void Warn(string format, params object[] args);
    }
}
