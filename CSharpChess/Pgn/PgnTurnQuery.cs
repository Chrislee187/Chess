// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace CSharpChess.Pgn
{
    public class PgnTurnQuery
    {
        public int Number { get; }
        public MoveQuery White { get; private set; }
        public MoveQuery Black { get; private set; }
        public string PgnSource { get; }

        public PgnTurnQuery(int turnNumber, MoveQuery white, MoveQuery black, string pgnSource = "")
        {
            Number = turnNumber;
            White = white;
            Black = black;
            PgnSource = pgnSource;
        }

        public override string ToString()
        {
            var w = White?.ToString() ?? "";
            var b = Black?.ToString() ?? "";

            if (White.GameOver) w = "end";
            if (Black.GameOver) b = "end";

            return $"{Number}. {w} {b}";
        }
    }
}