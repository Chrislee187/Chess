using CSharpChess.System;

namespace CsChess.Pgn
{
    public static class PgnResult
    {
        public static ChessGameResult Parse(string tagPair)
        {
            if(tagPair == "1/2-1/2") return ChessGameResult.Draw;
            if(tagPair == "1-0") return ChessGameResult.WhiteWins;
            if(tagPair == "0-1") return ChessGameResult.BlackWins;

            return ChessGameResult.Unknown;
        }
    }
}