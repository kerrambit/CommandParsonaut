using CommandParsonaut.Interfaces;

namespace RSSFeedifyCLIClient.IO
{
    /// <summary>
    /// Implements CommandParsonaut IReader interface.
    /// </summary>
    public class Reader : IReader
    {
        public void CursorLeft()
        {
            Console.CursorLeft--;
        }

        public void CursorRight()
        {
            Console.CursorLeft++;
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public ConsoleKeyInfo ReadSecretKey()
        {
            return Console.ReadKey(intercept: true);
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public bool IsAnyKeyAvailable()
        {
            return Console.KeyAvailable;
        }

        public int GetCursorLeftPosition()
        {
            return Console.CursorLeft;
        }
    }
}
