using System;
using System.Collections.Generic;
using CSharpChess;
using Chess = CSharpChess.Rules.Chess;

namespace CsChess.Pgn
{
    public static class PgnPieceName
    {
        public static Chess.PieceNames GetPieceName(char chr)
        {
            Chess.PieceNames result;

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
                result = Chess.PieceNames.Pawn;
            }
            return result;
        }
        private static readonly IDictionary<char, Chess.PieceNames> PgnNameMap = new Dictionary<char, Chess.PieceNames>
        {
            {'P', Chess.PieceNames.Pawn },
            {'N', Chess.PieceNames.Knight },
            {'B', Chess.PieceNames.Bishop },
            {'R', Chess.PieceNames.Rook },
            {'Q', Chess.PieceNames.Queen },
            {'K', Chess.PieceNames.King },
            {'O', Chess.PieceNames.King },
        };
    }
}