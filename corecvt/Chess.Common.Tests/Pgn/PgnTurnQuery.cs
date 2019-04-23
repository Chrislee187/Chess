// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace Chess.Common.Tests.Pgn
{
    public class PgnTurnQuery
    {
        public int Number { get; }
        public PgnQuery White { get; }
        public PgnQuery Black { get; }
        public string PgnSource { get; }

        public PgnTurnQuery(int turnNumber, PgnQuery white, PgnQuery black, string pgnSource = "")
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

            if (White?.GameOver ?? false) w = "end";
            if (Black?.GameOver ?? false) b = "end";

            return $"{Number}. {w} {b}";
        }
    }
}