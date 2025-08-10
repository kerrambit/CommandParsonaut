using CommandParsonaut.Core.Types;
using CommandParsonaut.Interfaces;
using CommandParsonaut.OtherToolkit;
using System.Text;

namespace CommandParsonaut.Core
{
    /// <summary>
    /// Stores all commands. If asked, it reads data from standart input and can check if any commands is equal and checks and parses also the parameters.
    /// Uses IWriter to print error message onto the given output environmemt and IReader to get the input.
    /// </summary>
    public class CommandParser
    {
        private IWriter _writer;
        private IReader _reader;
        private List<ICommand> _commands = new List<ICommand>();
        private int _maxCommandArgumentLength = 0;
        
        private TerminalHistory _terminalHistory = new();

        public event EventHandler<string>? InputGiven;
        public readonly string TerminalPromt = ">>> ";

        public struct CommandParserData
        {
            public ICommand Command;
            public IList<ParameterResult> Results;
            public string RawInput;

            public CommandParserData(ICommand command, IList<ParameterResult> results, string rawInput)
            {
                Command = command;
                Results = results;
                RawInput = rawInput;
            }
        }

        public CommandParser(IWriter writer, IReader reader)
        {
            _writer = writer;
            _reader = reader;
        }

        public void AddCommands(IList<ICommand> commands)
        {
            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }

        public void AddCommand(ICommand command)
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
                    else if (key.Key == ConsoleKey.End)
                    {
                        TerminalBasicAbilities.ExecuteCarriageReturnBackwards(_reader, TerminalPromt.Length + builder.Length);
                    }
                    else if (key.Key == ConsoleKey.Home)
                    {
                        TerminalBasicAbilities.ExecuteCarriageReturn(_reader, TerminalPromt.Length);
                    }
                    else if (key.Key == ConsoleKey.UpArrow)
                    {
                        var result = _terminalHistory.BackInHistory();
                        if (result.IsSuccess)
                        {
                            ReplaceCurrentLineWithCommand(builder, result.GetValue);
                        }
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        var result = _terminalHistory.ForwardInHistory();
                        if (result.IsSuccess)
                        {
                            ReplaceCurrentLineWithCommand(builder, result.GetValue);
                        } else
                        {
                            if (result.GetError == TerminalHistory.ForwardInHistorySuccess.BackAtTheBeginning)
                            {
                                TerminalBasicAbilities.ExecuteBackspace(_reader, _writer, builder.Length);
                                builder.Length = 0;
                            }
                        }
                    }
                    else if (key.Key == ConsoleKey.RightArrow)
                    {
                        TerminalBasicAbilities.ExecuteCursorMovemenet(_reader, TerminalBasicAbilities.CursorMovementDirection.Right, 1, rightIndent: builder.Length + TerminalPromt.Length);
                    }
                    else if (key.Key == ConsoleKey.LeftArrow)
                    {
                        TerminalBasicAbilities.ExecuteCursorMovemenet(_reader, TerminalBasicAbilities.CursorMovementDirection.Left, 1, leftIndent: TerminalPromt.Length);
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (builder.Length > 0)
                        {
                            int builderOffset = _reader.GetCursorLeftPosition() - TerminalPromt.Length;
                            TerminalBasicAbilities.ExecuteCursorMovemenet(_reader, TerminalBasicAbilities.CursorMovementDirection.Right, builder.Length - builderOffset);
                            TerminalBasicAbilities.ExecuteBackspace(_reader, _writer, count: builder.Length, leftIndent: TerminalPromt.Length);
                            builder = builder.Remove(builderOffset - 1, 1);
                            _writer.RenderBareText(builder.ToString(), newLine: false);
                            TerminalBasicAbilities.ExecuteCursorMovemenet(_reader, TerminalBasicAbilities.CursorMovementDirection.Left, builder.Length - builderOffset + 1);
                        }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        input = builder.ToString();
                        _writer.RenderBareText("");
                        if (input.Length > 0)
                        {
                            _terminalHistory.Add(input);
                        }
                        return true;
                    }
                    else
                    {
                        int builderOffset = _reader.GetCursorLeftPosition() - TerminalPromt.Length;
                        builder = builder.Insert(builderOffset, key.KeyChar);

                        TerminalBasicAbilities.ExecuteCursorMovemenet(_reader, TerminalBasicAbilities.CursorMovementDirection.Right, builder.Length - builderOffset);
                        TerminalBasicAbilities.ExecuteBackspace(_reader, _writer, count: builder.Length, leftIndent: TerminalPromt.Length);
                        _writer.RenderBareText(builder.ToString(), newLine: false);

                        TerminalBasicAbilities.ExecuteCursorMovemenet(_reader, TerminalBasicAbilities.CursorMovementDirection.Left, builder.Length - builderOffset - 1);
                    }
                }
            }
        }

        private void InvokeEventHandler(StringBuilder stringBuilder)
        {
            if (InputGiven is not null)
            {
                InputGiven.Invoke(this, stringBuilder.ToString());
            }
        }

        public Result<CommandParserData, string> GetCommand()
        {
            RenderTerminalPrompt();
            StringBuilder stringBuilder = new StringBuilder();

            string input;
            if (!GetUnprocessedInput(out input))
            {
                stringBuilder.Append($"RawInput: <{input}>. GetCommand() result: [ERROR] - unable to read input.");
                InvokeEventHandler(stringBuilder);
                return Result.Error<CommandParserData, string>("Unable to read input.");
            }
            stringBuilder.Append($"RawInput: <{input}>");

            input = input.Trim();
            string[] tokens = InputParser.SplitInput(input, true);
            if (tokens.Length <= 0)
            {
                RenderEmptyCommandMessage();
                stringBuilder.Append($". GetCommand() result: [ERROR] - empty input.");
                InvokeEventHandler(stringBuilder);
                return Result.Error<CommandParserData, string>("Empty input.");
            }
            stringBuilder.Append($"; Tokens: [{string.Join(", ", tokens.Select(token => $"<{token}>"))}]");
            stringBuilder.Append($"; CommandToken: <{tokens[0]}>");

            foreach (var command in _commands)
            {
                if (tokens[0].Equals(command.Name.ToUpper()) || tokens[0].Equals(command.Name.ToLower()) || tokens[0].Equals("help"))
                {
                    CommandParserData commandParserData = new CommandParserData();
                    commandParserData.RawInput = input;
                    commandParserData.Command = command;

                    if (tokens[0] == "help")
                    {
                        RenderHelp();
                        stringBuilder.Append($". GetCommand() result: [WARNING] - Help command was entered.");
                        commandParserData.Results = new List<ParameterResult>();
                        InvokeEventHandler(stringBuilder);
                        return Result.Error<CommandParserData, string>("Help command was entered.");
                    }

                    stringBuilder.Append($"; CommandToken identified with Command: <{command.ToString()}>");

                    string error;
                    if (!CheckCommandParameters(command, tokens, out error, out commandParserData.Results))
                    {
                        _writer.RenderErrorMessage(error);
                        stringBuilder.Append($". GetCommand() result: [ERROR] - parsing of arguments ended with error. Detailed message: '{error}'");
                        InvokeEventHandler(stringBuilder);
                        return Result.Error<CommandParserData, string>(error);
                    }

                    stringBuilder.Append($"; CommandArguments: <{string.Join(", ", commandParserData.Results)}>");
                    stringBuilder.Append($". GetCommand() result: [SUCCESS] - parsing of the command's arguments was successfull.");
                    InvokeEventHandler(stringBuilder);
                    return Result.Ok<CommandParserData, string>(commandParserData);
                }
            }

            RenderUnknownCommandMessage(in input);
            stringBuilder.Append($". GetCommand() result: [ERROR] - unknown command.");
            InvokeEventHandler(stringBuilder);
            return Result.Error<CommandParserData, string>($"Unknown command '{input}'.");
        }

        public static bool CheckCommandParameters(ICommand command, string[] tokens, out string error, out IList<ParameterResult> results)
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