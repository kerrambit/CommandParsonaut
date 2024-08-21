using CommandParsonaut.Core.Types;
using CommandParsonaut.Interfaces;
using CommandParsonaut.OtherToolkit;
using Example;

using ParameterResults = System.Collections.Generic.IList<CommandParsonaut.Core.Types.ParameterResult>;

namespace RSSFeedifyCLIClient.Repository
{
    /// <summary>
    /// Stores commands.
    /// </summary>
    public static class CommandsRepository
    {
        private static SomeData someData = new SomeData();

        private static void ReadPassword(IWriter writer, IReader reader)
        {
            PasswordReader passwordReader = new(reader, writer);
            writer.RenderBareText($"Please, enter the password: ", newLine: false);
            string password = passwordReader.ReadPassword();
            writer.RenderBareText($"You have entered the password: {password}");
        }

        public static IList<ICommand> InitCommands(IWriter writer, IReader reader)
        {
            var commands = new List<ICommand>();

            commands.Add(new Command(
                (IList<ParameterResult> list) => { writer.RenderBareText($"Sum is {list[0].Integer + list[1].Integer}."); },
                "integers-range",
                "a:INTEGER b:INTEGER", "Sums two integers (i = 0 to 1) in range <100 * i, 100 * (i + 1) - 1>.",
                new List<ParameterType> { ParameterType.IntegerRange, ParameterType.IntegerRange },
                new List<(int, int)> { (0, 99), (100, 199) }));

            commands.Add(new Command((IList<ParameterResult> list) => { writer.RenderBareText($"\"{someData.SomeState}\""); },
                "read-email",
                "",
                "Shows stored email."));

            commands.Add(new Command((IList<ParameterResult> list) => { someData.SomeState = list[0].Email; },
                "set-email",
                "email:EMAIL",
                "Enter valid email.",
                new List<ParameterType>() { ParameterType.Email }));

            commands.Add(new Command((ParameterResults list) => { writer.RenderBareText(list[0].Uri.ToString()); },
                "url",
                "url:URI",
                "Enter valid URL.", new List<ParameterType> { ParameterType.Uri }));

            commands.Add(new Command((ParameterResults list) => { writer.RenderBareText($"{list[0].Double + list[1].Double}"); },
                "doubles-range",
                "a:DOUBLE b:DOUBLE",
                "Sums two doubles (a is from <-49.5, 89.5> and b is from <0, 1005.05>).",
                new List<ParameterType>() { ParameterType.DoubleRange, ParameterType.DoubleRange },
                new List<(double, double)> { (-49.5, 89.5), (0, 1005.05) }));

            commands.Add(new Command((ParameterResults list) => { ReadPassword(writer, reader); },
                "password",
                "",
                "Will run password reader."));

            commands.Add(new Command(
                async (parameters) =>
                {
                    writer.RenderBareText("Async work... Work will finish in the background.");
                    await Task.Delay(3000);
                },
                "async",
                string.Empty,
                "Simulates async work."));

            return commands;
        }
    }
}
