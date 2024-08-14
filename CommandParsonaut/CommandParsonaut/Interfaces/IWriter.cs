namespace CommandParsonaut.Interfaces
{
    /// <summary>
    /// Offers basic method to render different type of messages.
    /// </summary>
    public interface IWriter
    {
        string Name { get; set; }

        void ClearTerminal();
        void RenderBareText(in string message, bool newLine = true);
        void RenderDebugMessage(in string message);
        void RenderErrorMessage(in string message);
        void RenderMessage(in string message);
        void RenderWarningMessage(in string message);
    }
}