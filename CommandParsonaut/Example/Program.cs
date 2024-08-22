using CommandParsonaut.Core;
using CommandParsonaut.Core.Types;
using CommandParsonaut.Interfaces;
using RSSFeedifyCLIClient.IO;
using RSSFeedifyCLIClient.Repository;

namespace RSSFeedifyCLIClient
{
    public class Application
    {
        /// <summary>
        /// Basic example on how to use CommandParsonaut class library.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            IWriter writer = new Writer();
            IReader reader = new Reader();

            var commands = CommandsRepository.InitCommands(writer, reader);

            var parser = new CommandParser(writer, reader);
            parser.AddCommands(commands);

            bool appRunning = true;
            parser.AddCommand(new Command((IList<ParameterResult> list) => { appRunning = false; }, "quit", "", "Quits the application.", new List<ParameterType> { }));

            parser.InputGiven += (sender, data) =>
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(data);
                Console.ForegroundColor = originalColor;
            };

            while (appRunning)
            {
                var result = parser.GetCommand();
                if (result.IsSuccess)
                {
                    IRunnableCommand runnable = new RunnableCommand(result.GetValue.Command, result.GetValue.Results);
                    runnable.Execute();
                }
            }
        }
    }
}
