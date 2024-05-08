using CommandParsonaut.Core.Types;

namespace RSSFeedifyCLIClient.Repository
{
    /// <summary>
    /// Stores all commands in the map.
    /// </summary>
    public static class CommandsRepository
    {
        public static Dictionary<string, Command> InitCommands()
        {
            Dictionary<string, Command> commands = new Dictionary<string, Command>();

            Command sum = new Command("sum", "a:INTEGER b:INTEGER c:INTEGER", "Sums three integers (i = 0 to 2) in range <100 * i, 100 * (i + 1) - 1>.", new List<ParameterType> { ParameterType.IntegerRange, ParameterType.IntegerRange, ParameterType.IntegerRange, }, new List<(int, int)> { (0, 99), (100, 199), (200, 299) });
            Command quit = new Command("quit", "", "Quits the application.", new List<ParameterType> { });

            commands["sum"] = sum;
            commands["quit"] = quit;

            return commands;
        }
    }
}
