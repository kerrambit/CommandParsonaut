namespace CommandParsonaut.Core.Types
{
    /// <summary>
    /// Encapsulates basic data about the command, such as name, parameters etc.
    /// </summary>
    public class Command
    {
        public string Name { get; } = string.Empty;
        public string ParametersInStrinFormat { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public IList<ParameterType> Parameters { get; } = new List<ParameterType>();
        public IList<IList<string>> Enums { get; } = new List<IList<string>>();
        public IList<(int min, int max)> Ranges { get; } = new List<(int, int)>();

        public Command() { }

        public Command(string name, string parametersInStringFormat, string description, IList<ParameterType> parameters)
        {
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
        }

        public Command(string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> ranges)
        {
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Ranges = ranges;
        }

        public Command(string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<IList<string>> enums)
        {
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Enums = enums;
        }

        public Command(string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> ranges, IList<IList<string>> enums)
        {
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Ranges = ranges;
            Enums = enums;
        }
    }
}
