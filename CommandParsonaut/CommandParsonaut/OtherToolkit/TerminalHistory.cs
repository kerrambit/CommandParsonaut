using CommandParsonaut.Core.Types;

namespace CommandParsonaut.OtherToolkit
{   
    /// <summary>
    /// This class encapsulates functionality of terminal history.
    /// </summary>
    public class TerminalHistory
    {
        private IList<string> _commandsHistory = new List<string>();
        private int _currentCommandsHistoryOffset = -1;

        public void Add(string command)
        {
            _commandsHistory.Add(command);
            _currentCommandsHistoryOffset = _commandsHistory.Count;
        }

        public Result<string, string> BackInHistory()
        {
            if (_currentCommandsHistoryOffset > 0 && _currentCommandsHistoryOffset <= _commandsHistory.Count)
            {
                _currentCommandsHistoryOffset--;
                string commandFromHistory = _commandsHistory[_currentCommandsHistoryOffset];
                return Result.Ok<string, string>(commandFromHistory);
            }

            return Result.Error<string, string>("");
        }

        public enum ForwardInHistorySuccess
        {
            BackAtTheBeginning,
            OutOfRange
        }

        public Result<string, ForwardInHistorySuccess> ForwardInHistory()
        {
            if (_currentCommandsHistoryOffset >= 0)
            {
                if (_currentCommandsHistoryOffset >= _commandsHistory.Count - 1)
                {
                    _currentCommandsHistoryOffset = _commandsHistory.Count;
                    return Result.Error<string, ForwardInHistorySuccess>(ForwardInHistorySuccess.BackAtTheBeginning);
                }
                else
                {
                    _currentCommandsHistoryOffset++;
                    string commandFromHistory = _commandsHistory[_currentCommandsHistoryOffset];
                    return Result.Ok<string, ForwardInHistorySuccess>(commandFromHistory);
                }
            }

            return Result.Error<string, ForwardInHistorySuccess>(ForwardInHistorySuccess.OutOfRange);
        }
    }
}
