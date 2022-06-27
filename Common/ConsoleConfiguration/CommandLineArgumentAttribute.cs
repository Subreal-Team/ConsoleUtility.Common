namespace SubRealTeam.ConsoleUtility.Common.ConsoleConfiguration
{
    /// <summary>
    /// Attribute defining command line configuration parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineArgumentAttribute : Attribute
    {
        /// <summary>
        /// Create command line configuration argument
        /// </summary>
        /// <param name="name">Command line parameter name</param>
        /// <param name="parseTemplate">Attribute value template. default: {name}={value}</param>
        /// <param name="defaultValue">Default value. default: ""</param>
        /// <param name="description">Description of the parameter.</param>
        public CommandLineArgumentAttribute(string name, string parseTemplate = "{name}={value}",
            object? defaultValue = null, string description = "")
        {
            Name = name;
            ParseTemplate = parseTemplate;
            DefaultValue = defaultValue;
            if (defaultValue != null)
            {
                DefaultValueIsSetup = true;
            }

            Description = description;
        }

        /// <summary>
        /// Command line parameter name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Attribute value template
        /// {name}={value} (default).
        ///   For example: /{name}:{value}
        /// </summary>
        public string ParseTemplate { get; }

        /// <summary>
        /// Default value
        /// </summary>
        public object? DefaultValue { get; }

        /// <summary>
        /// Indicates that default is specified
        /// </summary>
        public bool DefaultValueIsSetup { get; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; }
    }
}
