using CommandParsonaut.Core.Types;
using CommandParsonaut.Interfaces;
using CommandParsonaut.OtherToolkit;
using System.Text;

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
        private IList<string> _commandsHistory = new List<string>();
        private int _currentCommandsHistoryOffset = -1;

        public event EventHandler<string>? InputGiven;
        public readonly string TerminalPromt = ">>> ";

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

        private void ReplaceCurrentLineWithCommand(StringBuilder builder, string commandFromHistory)
        {
            TerminalBasicAbilities.ExecuteBackspace(_reader, _writer, builder.Length);
            builder.Length = 0;
            builder.Append(commandFromHistory);
            _writer.RenderBareText(commandFromHistory, newLine: false);
        }

        public bool GetUnprocessedInput(out string input)
        {
            var builder = new StringBuilder();

            while (true)
            {
                if (_reader.IsAnyKeyAvailable())
                {
                    // Handle special commands, such as CTRL+L. TODO: this will not be hardcoded, basically there will be ADirective, from which
                    // will inherit Command and KeyCombination. Command will read line, KeyCombination which replace current solution.
                    var key = _reader.ReadSecretKey();

                    if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.L)
                    {
                        _writer.ClearTerminal();
                        RenderTerminalPrompt();
                    }
                    else if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (_currentCommandsHistoryOffset > 0 && _currentCommandsHistoryOffset <= _commandsHistory.Count)
                        {
                            _currentCommandsHistoryOffset--;
                            string commandFromHistory = _commandsHistory[_currentCommandsHistoryOffset];
                            ReplaceCurrentLineWithCommand(builder, commandFromHistory);
                        }
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (_currentCommandsHistoryOffset >= 0)
                        {
                            if (_currentCommandsHistoryOffset >= _commandsHistory.Count - 1)
                            {
                                TerminalBasicAbilities.ExecuteBackspace(_reader, _writer, builder.Length);
                                builder.Length = 0;
                                _currentCommandsHistoryOffset = _commandsHistory.Count;
                            }
                            else
                            {
                                _currentCommandsHistoryOffset++;
                                string commandFromHistory = _commandsHistory[_currentCommandsHistoryOffset];
                                ReplaceCurrentLineWithCommand(builder, commandFromHistory);
                            }
                        }
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        TerminalBasicAbilities.ExecuteBackspace(_reader, _writer, leftIndent: TerminalPromt.Length);
                        if (builder.Length > 0)
                        {
                            builder.Length--;
                        }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        input = builder.ToString();
                        _writer.RenderBareText("");
                        if (input.Length > 0)
                        {
                            _commandsHistory.Add(input);
                            _currentCommandsHistoryOffset = _commandsHistory.Count;
                        }
                        return true;
                    }
                    else
                    {
                        builder.Append(key.KeyChar);
                        _writer.RenderBareText(key.KeyChar.ToString(), newLine: false);
                    }
                }
            }
        }

        public bool GetCommand(out Command receivedCommand, out IList<ParameterResult> results, out string unprocessedInput)
        {
            RenderTerminalPrompt();

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

            (int index, int integerRangeIndex, int doubleRangeIndex, int enumIndex) indices = (0, 0, 0, 0);
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
                        succ = InputParser.ParseIntegerInRange(tokenToRule.Token, command.IntegerRanges[indices.integerRangeIndex].min, command.IntegerRanges[indices.integerRangeIndex].max, out intInRangeResult);
                        tempResults.Add(new ParameterResult(intInRangeResult));
                        additionalErrorMessage = $"Expected value in range <{command.IntegerRanges[indices.integerRangeIndex].min}, {command.IntegerRanges[indices.integerRangeIndex].max}>!";
                        indices.integerRangeIndex++;
                        break;

                    case ParameterType.Double:
                        double doubleResult;
                        succ = InputParser.ParseDouble(tokenToRule.Token, out doubleResult);
                        tempResults.Add(new ParameterResult(doubleResult));
                        break;

                    case ParameterType.DoubleRange:
                        double doubleInRangeResult;
                        succ = InputParser.ParseDoubleInRange(tokenToRule.Token, command.DoubleRanges[indices.doubleRangeIndex].min, command.DoubleRanges[indices.doubleRangeIndex].max, out doubleInRangeResult);
                        tempResults.Add(new ParameterResult(doubleInRangeResult));
                        additionalErrorMessage = $"Expected value in range <{command.DoubleRanges[indices.doubleRangeIndex].min}, {command.DoubleRanges[indices.doubleRangeIndex].max}>!";
                        indices.doubleRangeIndex++;
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

                    case ParameterType.Uri:
                        Uri? uriResult;
                        succ = Uri.TryCreate(tokenToRule.Token, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                        if (uriResult != null)
                        {
                            tempResults.Add(new ParameterResult((Uri)uriResult));
                        }
                        break;

                    case ParameterType.Email:
                        string emailResult;
                        succ = InputParser.ParseEmail(tokenToRule.Token, out emailResult);
                        if (succ)
                        {
                            tempResults.Add(new ParameterResult(new ParameterResult.EmailParameterResultArgument(emailResult)));
                        }
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
            _writer.RenderBareText(TerminalPromt, false);
        }
    }
}