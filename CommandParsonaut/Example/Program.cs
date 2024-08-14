using CommandParsonaut.CommandHewAwayTool;
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
            var commands = CommandsRepository.InitCommands();
            IWriter writer = new Writer();
            IReader reader = new Reader();

            var parser = new CommandParser(writer, reader);
            parser.AddCommands(commands.Values.ToList());

            bool appRunning = true;
            while (appRunning)
            {
                Command receivedCommand;
                IList<ParameterResult> parameters;
                string unprocessedInput;
                if (parser.GetCommand(out receivedCommand, out parameters, out unprocessedInput))
                {
                    switch (receivedCommand.Name)
                    {
                        case "quit":
                            appRunning = false;
                            break;
                        case "integers-range":
                            writer.RenderBareText($"Entered command: <{unprocessedInput}>");
                            writer.RenderBareText($"Sum: {HandleIntegerSumCommand(parameters)}");
                            break;
                        case "doubles-range":
                            writer.RenderBareText($"Entered command: <{unprocessedInput}>");
                            writer.RenderBareText($"Sum: {HandleDoubleSumCommand(parameters)}");
                            break;
                        case "url":
                            writer.RenderBareText($"Entered command: <{unprocessedInput}>");
                            break;
                        case "email":
                            writer.RenderBareText($"Entered command: <{unprocessedInput}>");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static int HandleIntegerSumCommand(IList<ParameterResult> parameters)
        {
            int sum = 0;
            foreach (ParameterResult parameterResult in parameters)
            {
                sum += parameterResult.Integer;
            }

            return sum;
        }

        private static double HandleDoubleSumCommand(IList<ParameterResult> parameters)
        {
            double sum = 0.0;
            foreach (ParameterResult parameterResult in parameters)
            {
                sum += parameterResult.Double;
            }

            return sum;
        }
    }
}
