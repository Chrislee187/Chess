namespace chess.blazor.Extensions
{
    public static class ChessLocationHelpers
    {

        private static readonly int _asciiLowerCaseA = 'a';
        public static int ToBoardStringIdx(this (int x, int y) xy) => ((8 - xy.y) * 8) + xy.x - 1;
        public static string ToChessLocation(this (int x, int y) xy) => $"{(char)(xy.x + _asciiLowerCaseA - 1)}{xy.y}";
    }
}