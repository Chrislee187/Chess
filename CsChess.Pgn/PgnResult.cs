using CSharpChess.System;

namespace CsChess.Pgn
{
    public class PgnResult
    {
        public static ChessGameResult Parse(string tagPair)
        {
            if(tagPair == "1/2-1/2") return ChessGameResult.Draw;

            return ChessGameResult.Unknown;
        }
    }
}