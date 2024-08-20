
namespace CommandParsonaut.Core.Types
{
    public interface ICommand
    {
        CommandWorker Worker { get; }
        string Description { get; }
        IList<(double min, double max)> DoubleRanges { get; }
        IList<IList<string>> Enums { get; }
        IList<(int min, int max)> IntegerRanges { get; }
        string Name { get; }
        IList<ParameterType> Parameters { get; }
        string ParametersInStrinFormat { get; }
    }
}