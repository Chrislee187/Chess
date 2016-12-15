using System;
// ReSharper disable All

namespace CSharpChess.TheBoard
{
    public class PgnChessMove
    {
        public static ChessMove Parse(string pgnMove, ChessBoard board)
        {
            MoveType moveType;
            int idx = 0;
            if (IsKnownPiece(pgnMove[idx]))
            {

            }
            else
            {
                var pawnFile = (Chess.Board.ChessFile) Enum.Parse(typeof(Chess.Board.ChessFile), pgnMove[idx++].ToString(), true);
                int rank;
                if (pgnMove[idx] == 'x')
                {
                    moveType = MoveType.Take;
                    idx++;
                }
                if (!int.TryParse(pgnMove[idx].ToString(), out rank))
                {
//                    bored now, read up on it more later
                }

                
            }
            return null;
        }

        private static bool IsKnownPiece(char c)
        {
            throw new NotImplementedException();
        }
    }
}