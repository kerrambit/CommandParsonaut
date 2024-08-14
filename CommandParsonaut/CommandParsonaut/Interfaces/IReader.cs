
namespace CommandParsonaut.Interfaces
{
    /// <summary>
    /// Offers basic method to read the input.
    /// </summary>
    public interface IReader
    {
        string? ReadLine();
        ConsoleKeyInfo ReadKey();
        void CursorLeft();
        void CursorRight();
    }
}
