using System.Diagnostics;
using System.Xml.Schema;

namespace chess.pgn
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class PgnMove
    {
        private string DebuggerDisplayText => San;
        public PgnMove(string san, string comment)
        {
            San = san;
            Comment = comment;
        }

        public string San { get; }
        public string Comment { get; private set; }
    }
}