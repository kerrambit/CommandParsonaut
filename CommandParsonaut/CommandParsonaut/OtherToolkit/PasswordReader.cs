using CommandParsonaut.Interfaces;

namespace CommandParsonaut.OtherToolkit
{
    /// <summary>
    /// This class enables to read passwords.
    /// </summary>
    public class PasswordReader
    {
        private readonly IReader _reader;
        private readonly IWriter _writer;

        public PasswordReader(IReader reader, IWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo key;

            do
            {
                key = _reader.ReadSecretKey();

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    _writer.RenderBareText("*", newLine: false);
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Remove(password.Length - 1);

                    TerminalBasicAbilities.ExecuteBackspace(_reader, _writer);
                }

            } while (key.Key != ConsoleKey.Enter);

            _writer.RenderBareText("");
            return password;
        }
    }
}
