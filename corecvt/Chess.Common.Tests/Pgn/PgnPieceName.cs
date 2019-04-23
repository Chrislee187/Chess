using System;
using System.Collections.Generic;
using CSharpChess;

namespace Chess.Common.Tests.Pgn
{
    public static class PgnPieceName
    {
        public static PieceNames GetPieceName(char chr)
        {
            PieceNames result;

            if (char.IsUpper(chr))
            {
                var pieceChar = char.ToUpper(chr);
                if (!PgnNameMap.ContainsKey(pieceChar))
                {
                    throw new NotImplementedException($"'{pieceChar}' is not a valid SAN piece.");
                }
                result = PgnNameMap[pieceChar];
            }
            else
            {
                result = PieceNames.Pawn;
            }
            return result;
        }
        private static readonly IDictionary<char, PieceNames> PgnNameMap = new Dictionary<char, PieceNames>
        {
            {'P', PieceNames.Pawn },
            {'N', PieceNames.Knight },
            {'B', PieceNames.Bishop },
            {'R', PieceNames.Rook },
            {'Q', PieceNames.Queen },
            {'K', PieceNames.King },
            {'O', PieceNames.King },
        };
    }
}