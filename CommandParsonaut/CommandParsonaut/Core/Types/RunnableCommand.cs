
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
