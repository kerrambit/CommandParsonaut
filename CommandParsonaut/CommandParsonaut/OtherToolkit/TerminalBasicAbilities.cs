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

        public enum CursorMovementDirection
        {
            Left,
            Right
        }

        public static void ExecuteCursorMovemenet(IReader reader, CursorMovementDirection direction, int count = 1, int leftIndent = 0, int rightIndent = int.MaxValue)
        {
            for (int i = 0; i < count; i++)
            {
                if (reader.GetCursorLeftPosition() > leftIndent && reader.GetCursorLeftPosition() < rightIndent)
                {
                    switch (direction)
                    {
                        case CursorMovementDirection.Left:
                            reader.CursorLeft();
                            break;
                        case CursorMovementDirection.Right:
                            reader.CursorRight();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public static void ExecuteCarriageReturn(IReader reader, int leftIndent)
        {
            while (reader.GetCursorLeftPosition() > leftIndent)
            {
                reader.CursorLeft();
            }
        }

        public static void ExecuteCarriageReturnBackwards(IReader reader, int rightIndent)
        {
            while (reader.GetCursorLeftPosition() < rightIndent)
            {
                reader.CursorRight();
            }
        }
    }
}
