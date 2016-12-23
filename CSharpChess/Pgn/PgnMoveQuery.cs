using System;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.Pgn
{
    /*
     * Pgn notation requires current board state to determine the actual move
     * as it is not explicit about the piece being moved only what happened and
     * where the destination was.
     * 
     * We will parse the raw text to create a moveQuery to apply against the board
     * that can be used to determine the missing details
     * 
     */

    public class PgnMoveQuery
    {
        public MoveType MoveType { get; }
        public ChessPiece Piece { get; }
        public BoardLocation Destination { get; }

        private PgnMoveQuery(ChessPiece piece, BoardLocation destination, MoveType moveType)
        {
            Destination = destination;
            Piece = piece;
            MoveType = moveType;
        }

        public static bool TryParse(Chess.Colours turn, string move, out PgnMoveQuery moveQuery)
        {
            Chess.PieceNames pn = Chess.PieceNames.Blank;
            MoveType moveType = MoveType.Unknown;
            
            string dest = "";
            var firstChar = move[0];
            if (char.IsUpper(firstChar))
            {
                throw new NotImplementedException("Non-pawn PGN moves not yet implemented");
            }

            pn = GetPieceName(firstChar);

            if (move.Length == 2)
            {
                dest = move;
                moveType = MoveType.Move;
            }
            else
            {
                var secondChar = move[1];
                throw new NotImplementedException("Only Moves for pawns are supported not takes");
            }
            var piece = new ChessPiece(turn, pn);
            var destination = BoardLocation.At(dest);

            moveQuery = new PgnMoveQuery(piece, destination, moveType);
            return true;
        }

        private static Chess.PieceNames GetPieceName(char pieceChar)
        {
            return Chess.PieceNames.Pawn;
        }
    }
}