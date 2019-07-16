using System;
using System.Collections.Generic;
using System.Linq;

namespace SubrealTeam.Common.Logging
{
    /// <summary>
    /// Singleton logging class
    /// </summary>
    public static class Logger
    {
        private static IList<ILogger> Instances { get; } = new List<ILogger>();

        /// <summary>
        /// Add new Instance for logging
        /// </summary>
        /// <param name="logger"></param>
        public static void AddInstance(ILogger logger)
        {
            Guard.IsNotNull(logger, nameof(logger));

            if (Instances.All(x => x.GetType() != logger.GetType()))
            {
                Instances.Add(logger);
            }
        }

        /// <summary> Logging (DEBUG level) formatted string </summary>
        public static void Debug(string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Debug(format, args);
            }
        }

        /// <summary> Logging (ERROR level) of formatted string </summary>
        public static void Error(string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Error(format, args);
            }
        }

        /// <summary> Logging (ERROR level) exceptions with a message, if the message is not specified - logging an exception </summary>
        public static void Error(Exception exception, string message = null)
        {
            foreach (var instance in Instances)
            {
                instance.Error(exception, message);
            }
        }

        /// <summary> Logging (ERROR level) exception with message (formatted string) </summary>
        public static void Error(Exception exception, string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Error(exception, format, args);
            }
        }

        /// <summary> Logging (FATAL level) formatted string </summary>
        public static void Fatal(string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Fatal(format, args);
            }
        }

        /// <summary> Logging (FATAL level) exceptions with a message, if the message is not specified - exception logging </summary>
        public static void Fatal(Exception exception, string message = null)
        {
            foreach (var instance in Instances)
            {
                instance.Fatal(exception, message);
            }
        }

        /// <summary> Logging (FATAL level) exceptions with message (formatted string) </summary>
        public static void Fatal(Exception exception, string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Fatal(exception, format, args);
            }
        }

        /// <summary> Logging (INFO level) of formatted string </summary>
        public static void Info(string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Info(format, args);
            }
        }

        /// <summary> Logging (WARN level) formatted string </summary>
        public static void Warn(string format, params object[] args)
        {
            foreach (var instance in Instances)
            {
                instance.Warn(format, args);
            }
        }
    }
}
