using CommandParsonaut.Interfaces;

namespace CommandParsonaut.OtherToolkit
{
    public static class TerminalBasicAbilities
    {
        public static void ExecuteBackspace(IReader reader, IWriter writer, int count = 1, int leftIndent = 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (reader.GetCursorLeftPosition() > leftIndent)
                {
                    reader.CursorLeft();
                    writer.RenderBareText(" ", newLine: false);
                    reader.CursorLeft();
                }
                else
                {
                    break;
                }
            }
        }
    }
}
