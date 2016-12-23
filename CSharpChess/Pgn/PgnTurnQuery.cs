// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace CSharpChess.Pgn
{
    public class PgnTurnQuery
    {
        public int Number { get; }
        public PgnMoveQuery White { get; private set; }
        public PgnMoveQuery Black { get; private set; }
        public string PgnSource { get; }

        public PgnTurnQuery(int turnNumber, PgnMoveQuery white, PgnMoveQuery black, string pgnSource)
        {
            Number = turnNumber;
            White = white;
            Black = black;
            PgnSource = pgnSource;
        }

        public override string ToString()
        {
            var w = White?.Destination.ToString() ?? "";
            var b = Black?.Destination.ToString() ?? "";
            return $"{Number}. {w} {b}";
        }
    }
}