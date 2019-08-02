using System;
using SubRealTeam.ConsoleUtility.Common.Extensions;

namespace SubRealTeam.ConsoleUtility.Common
{
    /// <summary>
    /// Implements methods for protecting parameters and local variables.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Causes an exception of type <typeparamref name = "TException"/> with the specified message
        /// when the statement is true.
        /// </summary>
        /// <typeparam name="TException">The type of exception being thrown.</typeparam>
        /// <param name="assertion">Condition to check. If true, then an exception is thrown. <typeparamref name="TException"/>.</param>
        /// <param name="message">The message string for the exception being called.</param>
        public static void Against<TException>(bool assertion, string message) where TException : Exception
        {
            if (assertion)
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// Causes an exception of type <typeparamref name = "TException"/> with the specified message
        /// when the statement is true.
        /// </summary>
        /// <typeparam name="TException">The type of exception being thrown.</typeparam>
        /// <param name="assertion">Condition to check. If true, then an exception is thrown. <typeparamref name="TException"/>.</param>
        /// <param name="message">The message string for the exception being called.</param>
        public static void Against<TException>(Func<bool> assertion, string message) where TException : Exception
        {
            //Execute the lambda and if it evaluates to true then throw the exception.
            if (assertion())
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// Causes <see cref="InvalidOperationException"/> when the specified instance
        /// it is not inherited directly from type <typeparamref name="TBase"/>.
        /// </summary>
        /// <typeparam name="TBase">Basic type for check</typeparam>
        /// <param name="instance">Object for which inheritance from type is checked <typeparamref name="TBase"/>.</param>
        /// <param name="paramName">Name of a instance</param>
        /// <param name="message">Message string for the exception being called</param>
        public static void InheritsFrom<TBase>(object instance, string paramName = "",
            string message = "Instance {0} not inherited directly from type {1}") // where TBase : Type
        {
            InheritsFrom<TBase>(instance.GetType(), message.Args(paramName, typeof(TBase)));
        }

        /// <summary>
        /// Causes <see cref="InvalidOperationException"/> when the specified type
        /// it is not inherited directly from type <typeparamref name="TBase"/>.
        /// </summary>
        /// <typeparam name="TBase">Base type to check</typeparam>
        /// <param name="type">Type <see cref = "Type"/> to test inheritance type <typeparamref name="TBase"/>.</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void InheritsFrom<TBase>(Type type, string message = "")
        {
            if (type.BaseType != typeof(TBase))
                throw new InvalidOperationException(
                    message.NotEmpty("Type {0} not inherited directly from type {1}".Args(type, typeof(TBase))));
        }

        /// <summary>
        /// Causes <see cref="InvalidOperationException"/> when the specified instance
        /// it is not inherited directly from type <typeparamref name="TBase"/>.
        /// </summary>
        /// <typeparam name="TBase">Base type to check</typeparam>
        /// <param name="instance">The object for which inheritance from the type is checked <typeparamref name="TBase"/>.</param>
        /// <param name="paramName">Name of a instance</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void IsSubclassOf<TBase>(object instance, string paramName = "",
            string message = "Instance {0} not inherited from type {1}")
        {
            if (!instance.GetType().IsSubclassOf(typeof(TBase)))
                throw new InvalidOperationException(message.Args(paramName, typeof(TBase)));
        }

        /// <summary>
        /// Causes <see cref="InvalidOperationException"/> when the specified instance
        /// does not implement interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface that should implement the instance</typeparam>
        /// <param name="instance">Instance for which interface implementation is checked <typeparamref name="TInterface"/>.</param>
        /// <param name="paramName">Name of a instance</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void Implements<TInterface>(object instance, string paramName = "",
            string message = "Instance {0} does not implement interface {1}")
        {
            Implements<TInterface>(instance.GetType(), message.Args(paramName, typeof(TInterface)));
        }

        /// <summary>
        /// Causes <see cref="InvalidOperationException"/> when specified type
        /// does not implement interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of interface <paramref name = "type"/> that should be implemented</typeparam>
        /// <param name="type">The type <see cref = "Type"/>, for which the implementation of the interface is checked <typeparamref name="TInterface"/>.</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void Implements<TInterface>(Type type, string message = "")
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
                throw new InvalidOperationException(
                    message.NotEmpty("Type {0} does not implement interface {1}".Args(type, typeof(TInterface))));
        }

        /// <summary>
        /// Causes <see cref="InvalidOperationException"/> when the specified instance
        /// is not specified type.
        /// </summary>
        /// <typeparam name="TType">The type for which the <paramref name="instance"/> is checked</typeparam>
        /// <param name="instance">The instance to be checked for</param>
        /// <param name="paramName">Name of a instance</param>
        /// <param name="message">Message for exception <see cref="InvalidOperationException"/>.</param>
        public static void TypeOf<TType>(object instance, string paramName = "",
            string message = "Instance {0} is not specified type {1}")
        {
            if (!(instance is TType))
                throw new InvalidOperationException(message.Args(paramName, typeof(TType)));
        }

        /// <summary>
        /// Throws an exception when an object instance is not equal to another object instance
        /// </summary>
        /// <typeparam name="TException">The type of exception being thrown</typeparam>
        /// <param name="compare">Comparing instance object</param>
        /// <param name="instance">The instance of the object to compare with</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void IsEqual<TException>(object compare, object instance, string message)
            where TException : Exception
        {
            if (compare != instance)
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>
        /// Throws an Exception when the object instance is not equal to another object instance
        /// </summary>
        /// <param name="compare">Comparing instance object</param>
        /// <param name="instance">The instance of the object to compare with</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void IsEqual(object compare, object instance, string message)
        {
            IsEqual<Exception>(compare, instance, message);
        }

        /// <summary>
        /// Throws ArgumentNullException when the object instance is null
        /// </summary>
        /// <param name="instance">Object instance</param>
        /// <param name="paramName">The name of the parameter that caused this exception.</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void IsNotNull(object instance, string paramName = "",
            string message = "Instance is null")
        {
            if (ReferenceEquals(instance, null))
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws ArgumentNullException when the specified string is a null value, an empty string, or a string consisting of whitespace characters only.
        /// </summary>
        /// <param name="value">Specified string</param>
        /// <param name="paramName">The name of the parameter that caused this exception</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void IsNotEmpty(string value, string paramName = "", string message = "String not filled")
        {
            if (value.IsEmpty())
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws ArgumentNullException when the specified id is null or Guid.Empty
        /// </summary>
        /// <param name="value">Specified GUID</param>
        /// <param name="paramName">The name of the parameter that caused this exception</param>
        /// <param name="message">The message string for the exception being called</param>
        public static void IsNotEmpty(Guid value, string paramName = "", string message = "GUID is empty")
        {
            if (value == null || value == Guid.Empty)
                throw new ArgumentNullException(paramName, message);
        }
    }
}
