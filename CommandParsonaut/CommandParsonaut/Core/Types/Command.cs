namespace CommandParsonaut.Core.Types
{
    /// <summary>
    /// Encapsulates basic data about the command, such as name, parameters etc.
    /// It holds also worker which is function that definies what will be run when the command is entered.
    /// </summary>
    public class Command : ICommand
    {
        public CommandWorker Worker { get; protected set; }
        public string Name { get; } = string.Empty;
        public string ParametersInStrinFormat { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public IList<ParameterType> Parameters { get; } = new List<ParameterType>();
        public IList<IList<string>> Enums { get; } = new List<IList<string>>();
        public IList<(int min, int max)> IntegerRanges { get; } = new List<(int, int)>();
        public IList<(double min, double max)> DoubleRanges { get; } = new List<(double, double)>();

        public override string ToString()
        {
            return $"Command '{Name}': '{Description}'";
        }

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description)
        {
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Worker = worker;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges)
        {

            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<IList<string>> enums)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Enums = enums;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Enums = enums;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            Enums = enums;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges, IList<IList<string>> enums)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
            Enums = enums;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
            Enums = enums;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integerRanges, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            IntegerRanges = integerRanges;
            Enums = enums;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges, IList<IList<string>> enums)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            Enums = enums;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            Enums = enums;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(double min, double max)> doubleRanges, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            Enums = enums;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integersRanges, IList<(double min, double max)> doubleRanges)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            IntegerRanges = integersRanges;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integersRanges, IList<(double min, double max)> doubleRanges)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            IntegerRanges = integersRanges;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integersRanges, IList<(double min, double max)> doubleRanges)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            IntegerRanges = integersRanges;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        public Command(CommandWorker worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integersRanges, IList<(double min, double max)> doubleRanges, IList<IList<string>> enums)
        {
            Worker = worker;
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            IntegerRanges = integersRanges;
            Enums = enums;
        }

        public Command(Action<IList<ParameterResult>> worker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integersRanges, IList<(double min, double max)> doubleRanges, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(worker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            IntegerRanges = integersRanges;
            Enums = enums;
        }

        public Command(Func<IList<ParameterResult>, Task> asyncWorker, string name, string parametersInStringFormat, string description, IList<ParameterType> parameters, IList<(int min, int max)> integersRanges, IList<(double min, double max)> doubleRanges, IList<IList<string>> enums)
        {
            Worker = new CommandWorker(asyncWorker);
            Name = name;
            ParametersInStrinFormat = parametersInStringFormat;
            Description = description;
            Parameters = parameters;
            DoubleRanges = doubleRanges;
            IntegerRanges = integersRanges;
            Enums = enums;
        }
    }
}
