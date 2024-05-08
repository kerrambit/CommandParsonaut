using CommandParsonaut.Core.Types;
using CommandParsonaut.Interfaces;

namespace CommandParsonaut.CommandHewAwayTool
{
    /// <summary>
    /// Stores all commands. If asked, it reads data from standart input and can check if any commands is equal and checks and parses also the parameters.
    /// Uses IWriter to print error message onto the given output environmemt and IReader to get the input.
    /// </summary>
    public class CommandParser
    {
        private IWriter _writer;
        private IReader _reader;
        private List<Command> _commands = new List<Command>();
        private int _maxCommandArgumentLength = 0;

        public event EventHandler<string>? InputGiven;

        public CommandParser(IWriter writer, IReader reader)
        {
            _writer = writer;
            _reader = reader;
        }

        public void AddCommands(IList<Command> commands)
        {
            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }

        private void AddCommand(Command command)
        {
            _commands.Add(command);
            if (command.Name.Length + command.ParametersInStrinFormat.Length > _maxCommandArgumentLength)
            {
                _maxCommandArgumentLength = command.Name.Length + command.ParametersInStrinFormat.Length;
            }
        }

        public bool GetUnprocessedInput(out string input)
        {
            RenderTerminalPrompt();

            string? rawInput = _reader.ReadLine();
            if (rawInput == null)
            {
                input = string.Empty;
                return false;
            }

            input = rawInput;
            return true;
        }

        public bool GetCommand(out Command receivedCommand, out IList<ParameterResult> results, out string unprocessedInput)
        {
            string input;
            if (!GetUnprocessedInput(out input))
            {
                receivedCommand = new();
                results = [];
                unprocessedInput = input;
                return false;
            }

            if (InputGiven is not null)
            {
                InputGiven.Invoke(this, input);
            }

            input = input.Trim();
            string[] tokens = InputParser.SplitInput(input, true);
            if (tokens.Length <= 0)
            {
                RenderEmptyCommandMessage();
                receivedCommand = new();
                results = [];
                unprocessedInput = input;
                return false;
            }

            foreach (var command in _commands)
            {
                if (tokens[0].Equals(command.Name.ToUpper()) || tokens[0].Equals(command.Name.ToLower()) || tokens[0].Equals("help"))
                {
                    unprocessedInput = input;
                    receivedCommand = command;

                    if (tokens[0] == "help")
                    {
                        RenderHelp();
                        results = new List<ParameterResult>();
                        return false;
                    }

                    string error;
                    if (!CheckCommandParameters(command, tokens, out error, out results))
                    {
                        _writer.RenderErrorMessage(error);
                        return false;
                    }

                    return true;
                }
            }

            RenderUnknownCommandMessage(in input);
            receivedCommand = new();
            results = [];
            unprocessedInput = input;
            return false;
        }

        public static bool CheckCommandParameters(Command command, string[] tokens, out string error, out IList<ParameterResult> results)
        {
            error = string.Empty;
            if (command.Parameters.Count != tokens.Length - 1)
            {
                error = $"invalid number of arguments! Expected {command.Parameters.Count}, got {tokens.Length - 1}. Use Help if needed.";
                results = new List<ParameterResult>();
                return false;
            }

            tokens = tokens[1..];
            var mappedTokensToRules = tokens.Zip(command.Parameters, (token, rule) => new { Token = token, Rule = rule });

            (int index, int rangeIndex, int enumIndex) indices = (0, 0, 0);
            List<ParameterResult> tempResults = new List<ParameterResult>();
            foreach (var tokenToRule in mappedTokensToRules)
            {
                bool succ = true;
                string additionalErrorMessage = string.Empty;

                switch (tokenToRule.Rule)
                {
                    case ParameterType.Integer:
                        int intResult;
                        succ = InputParser.ParseInteger(tokenToRule.Token, out intResult);
                        tempResults.Add(new ParameterResult(intResult));
                        break;
                    case ParameterType.IntegerRange:
                        int intInRangeResult;
                        succ = InputParser.ParseIntegerInRange(tokenToRule.Token, command.Ranges[indices.rangeIndex].min, command.Ranges[indices.rangeIndex].max, out intInRangeResult);
                        tempResults.Add(new ParameterResult(intInRangeResult));
                        additionalErrorMessage = $"Expected value in range <{command.Ranges[indices.rangeIndex].min}, {command.Ranges[indices.rangeIndex].max}>!";
                        indices.rangeIndex++;
                        break;
                    case ParameterType.Double:
                        double doubleResult;
                        succ = InputParser.ParseDouble(tokenToRule.Token, out doubleResult);
                        tempResults.Add(new ParameterResult(doubleResult));
                        break;
                    case ParameterType.String:
                        tempResults.Add(new ParameterResult(tokenToRule.Token));
                        break;
                    case ParameterType.Enum:
                        succ = command.Enums[indices.enumIndex].Contains(tokenToRule.Token);
                        tempResults.Add(new ParameterResult(tokenToRule.Token));
                        additionalErrorMessage = $"Expected value from [{string.Join(", ", command.Enums[indices.enumIndex])}]!";
                        indices.enumIndex++;
                        break;
                    default:
                        break;
                }

                indices.index++;

                if (!succ)
                {
                    error = $"{indices.index}. parameter '{tokenToRule.Token}' is invalid. Type {tokenToRule.Rule} was expected. " + additionalErrorMessage;
                    results = new List<ParameterResult>();
                    return false;
                }
            }

            results = tempResults;
            return true;
        }

        private void RenderUnknownCommandMessage(in string command)
        {
            if (_commands.Count == 0)
            {
                _writer.RenderErrorMessage($"It seems that no commands were inserted into the parser. Try to use first AddCommand() or AddCommand().");
            }
            else
            {
                _writer.RenderErrorMessage($"Command '{command}' is unknown. Use Help if needed.");
            }
        }

        private void RenderEmptyCommandMessage()
        {
            _writer.RenderWarningMessage("Empty command is not allowed.");
        }

        private void RenderHelp()
        {
            _writer.RenderBareText($"\nCommand:{string.Concat(Enumerable.Repeat(" ", _maxCommandArgumentLength))}Description:\n");

            foreach (var command in _commands)
            {
                string commandWithParameters = (command.Name + " " + command.ParametersInStrinFormat).PadRight(_maxCommandArgumentLength + 6);
                _writer.RenderBareText($"{commandWithParameters} - {command.Description}");
            }

            _writer.RenderBareText("");
        }

        private void RenderTerminalPrompt()
        {
            _writer.RenderBareText($">>> ", false);
        }
    }
}