
namespace CommandParsonaut.Core.Types
{
    /// <summary>
    /// This class implements IRunnableCommand interface, which means that is ready to be executed.
    /// It needs to have a list of parsed parameters and the given command.
    /// </summary>
    public class RunnableCommand : Command, IRunnableCommand
    {
        private IList<ParameterResult> _results;

        public RunnableCommand(ICommand command, IList<ParameterResult> results)
            : base(command.Worker, command.Name, command.ParametersInStrinFormat, command.Description)
        {
            _results = results;
        }

        public async void Execute()
        {
            if (Worker.IsAsync)
            {
                await Worker.GetAsyncWorker().Invoke(_results);
            }
            else
            {
                Worker.GetWorker().Invoke(_results);
            }
        }
    }
}
