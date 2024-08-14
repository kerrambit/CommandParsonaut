using CommandParsonaut.Interfaces;

namespace RSSFeedifyCLIClient.IO
{
    /// <summary>
    /// Implements CommandHewAwayTool IReader interface.
    /// </summary>
    public class Reader : IReader
    {
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
