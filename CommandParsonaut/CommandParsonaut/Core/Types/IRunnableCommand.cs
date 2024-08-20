
namespace CommandParsonaut.Core.Types
{
    public interface IRunnableCommand : ICommand
    {
        void Execute();
    }
}
