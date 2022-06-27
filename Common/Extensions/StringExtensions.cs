using System;
using System.Linq;

namespace SubRealTeam.ConsoleUtility.Common.Extensions
{
    /// <summary>
    /// String Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats a string with parameters using string.Format.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        /// <returns>Formatted string</returns>
        public static string Args(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        #region IsEmpty, IsNotEmpty

        /// <summary>
        /// Indicates whether the specified string is a null value, an empty string, or a string consisting of whitespace characters only.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Indicates that the specified string is NOT a null value, an empty string or a string consisting only of whitespace characters.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }

        #endregion

        #region NotNull, NotEmpty

        /// <summary>
        /// Returns an empty string if the specified string is a null value, otherwise the string itself
        /// </summary>
        /// <param name="target">Target string</param>
        /// <returns></returns>
        public static string NotNull(this string target)
        {
            return target ?? string.Empty;
        }

        /// <summary>
        /// Returns the default value if the specified string is empty, otherwise the string itself
        /// </summary>
        /// <param name = "value">The string to check.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>Either the string or the default value.</returns>
        public static string NotEmpty(this string value, string defaultValue)
        {
            return value.IsNotEmpty() ? value : defaultValue;
        }

        #endregion

        #region ContainsAny, ContainsAll

        /// <summary>
        /// Checks for the occurrence of any of the specified lines in the given instance.
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string inputValue, params string[] values)
        {
            return inputValue.IsNotEmpty() && values.Any(p => inputValue.IndexOf(p, StringComparison.Ordinal) != -1);
        }

        /// <summary>
        /// Checks the entry of all the specified rows in the given instance.
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool ContainsAll(this string inputValue, params string[] values)
        {
            return inputValue.IsNotEmpty() && values.All(p => inputValue.IndexOf(p, StringComparison.Ordinal) != -1);
        }

        #endregion

        #region LikeAny, Like

        /// <summary>
        /// Checks if the string matches any of the specified patterns.
        /// </summary>
        /// <param name="value">String <see cref="System.String"/> object</param>
        /// <param name="patterns">patterns</param>
        /// <returns>Matching flag</returns>
        public static bool LikeAny(this string value, params string[] patterns)
        {
            return patterns.Any(value.Like);
        }

        /// <summary>
        /// Verifies that the string matches the specified pattern.
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="pattern">pattern</param>
        /// <returns>Matching flag</returns>
        public static bool Like(this string value, string pattern)
        {
            if (value == pattern || pattern == "*") return true;

            if (pattern.StartsWith("*"))
            {
                return value.Where((t, index) => value.Substring(index).Like(pattern.Substring(1))).Any();
            }

            if (pattern[0] == value[0])
            {
                return value.Substring(1).Like(pattern.Substring(1));
            }

            return false;
        }

        #endregion

    }
}
