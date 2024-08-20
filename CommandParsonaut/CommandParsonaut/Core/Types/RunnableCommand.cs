
namespace CommandParsonaut.Core.Types
{
    public class RunnableCommand : Command, IRunnableCommand
    {
        private IList<ParameterResult> _results;

        public RunnableCommand(ICommand command, IList<ParameterResult> results)
            : base(command.Worker, command.Name, command.ParametersInStrinFormat, command.Description)
        {
            _results = results;
        }

        public void Execute()
        {
            Worker(_results);
        }
    }
}
