using System.Diagnostics;

namespace chess.pgn
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class PgnTurn
    {
        private string DebuggerDisplayText => $"{Turn}. {White?.San ?? "..."} {Black?.San ?? "..."}";
        public int Turn { get; }
        public PgnMove White { get; }
        public PgnMove Black { get; }

        public PgnTurn(int turn, PgnMove white, PgnMove black)
        {
            Turn = turn;
            White = white;
            Black = black;
        }

    }
}