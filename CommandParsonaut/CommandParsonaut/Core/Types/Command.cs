
using ParameterResults = System.Collections.Generic.IList<CommandParsonaut.Core.Types.ParameterResult>;

namespace CommandParsonaut.Core.Types
{
    ///// <summary>
    ///// Encapsulates basic data about the command, such as name, parameters etc.
    ///// </summary>
    public class Command : ICommand
    {
        public Action<IList<ParameterResult>> Worker { get; protected set; }

        public string Name { get; } = string.Empty;
        public string ParametersInStrinFormat { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public IList<ParameterType> Parameters { get; } = new List<ParameterType>();
        public IList<IList<string>> Enums { get; } = new List<IList<string>>();
        public IList<(int min, int max)> IntegerRanges { get; } = new List<(int, int)>();
        public IList<(double min, double max)> DoubleRanges { get; } = new List<(double, double)>();

        public Command(Action<IList<ParameterResult>> worker)
        {
            Worker = worker;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges)
        {

            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<IList<string>> enums)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Enums = enums;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges, IList<IList<string>> enums)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
            Enums = enums;
        }
    }
}
