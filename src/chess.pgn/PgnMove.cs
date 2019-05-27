using System.Diagnostics;

namespace chess.pgn
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class PgnMove
    {
#if DEBUG
        private string DebuggerDisplayText => San;
#endif
        public PgnMove(string san, string comment)
        {
            San = san;

            // NOTE: Wouldn't normally set a string specifically to null, would almost certainly 
            // cause problems down the line, however this is only used as part of a parse mechanism
            // and as such this represents the parsing of an optional value more accurately.
            Comment = string.IsNullOrEmpty(comment) ? null : comment;
        }

        public string San { get; }
        public string Comment { get; private set; }
    }
}