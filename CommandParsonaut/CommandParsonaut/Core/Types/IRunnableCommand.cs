
namespace CommandParsonaut.Core.Types
{
    /// <summary>
    /// Defines the interface for command which is ready to be executed.
    /// </summary>
    public interface IRunnableCommand : ICommand
    {
        void Execute();
    }
}
