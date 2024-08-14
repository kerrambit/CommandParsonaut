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

            Command sumIntegers = new Command("integers-range", "a:INTEGER b:INTEGER c:INTEGER", "Sums three integers (i = 0 to 2) in range <100 * i, 100 * (i + 1) - 1>.", new List<ParameterType> { ParameterType.IntegerRange, ParameterType.IntegerRange, ParameterType.IntegerRange, }, new List<(int, int)> { (0, 99), (100, 199), (200, 299) });
            Command url = new Command("url", "url:URI", "Enter valid URL.", new List<ParameterType> { ParameterType.Uri });
            Command sumDoubles = new Command("doubles-range", "a:DOUBLE b:DOUBLE", "Sums three doubles (a is from <-49.5, 89.5> and b is from <0, 1005.05>).", new List<ParameterType>() { ParameterType.DoubleRange, ParameterType.DoubleRange }, new List<(double, double)> { (-49.5, 89.5), (0, 1005.05) });
            Command email = new Command("email", "email:EMAIL", "Enter valid email.", new List<ParameterType>() { ParameterType.Email });
            Command password = new Command("password", "", "Will run password reader.");
            Command quit = new Command("quit", "", "Quits the application.", new List<ParameterType> { });

            commands["integers-range"] = sumIntegers;
            commands["doubles-range"] = sumDoubles;
            commands["url"] = url;
            commands["email"] = email;
            commands["password"] = password;
            commands["quit"] = quit;

            return commands;
        }
    }
}
