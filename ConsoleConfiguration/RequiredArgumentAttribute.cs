using System;

namespace SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration
{
    /// <summary>
    /// Attribute defining required command line configuration parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class RequiredArgumentAttribute : Attribute
    {
    }
}