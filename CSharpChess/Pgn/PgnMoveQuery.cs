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
        public Chess.Board.ChessFile FromFile { get; }
        public MoveType MoveType { get; }
        public ChessPiece Piece { get; }
        public BoardLocation Destination { get; }

        private PgnMoveQuery(ChessPiece piece, BoardLocation destination, MoveType moveType, Chess.Board.ChessFile fromFile = Chess.Board.ChessFile.None)
        {
            Destination = destination;
            Piece = piece;
            MoveType = moveType;
            FromFile = fromFile;
        }

        public static bool TryParse(Chess.Colours turn, string move, out PgnMoveQuery moveQuery)
        {
            Chess.PieceNames pn = Chess.PieceNames.Blank;
            MoveType moveType = MoveType.Unknown;
            Chess.Board.ChessFile fromFile = Chess.Board.ChessFile.None;
            
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
                if (secondChar == 'x')
                {
                    moveType = MoveType.Take;
                }

                if (move.Length == 4)
                    dest = move.Substring(2, 2);

                fromFile = BoardLocation.At($"{move[0]}1").File;
                
            }
            var piece = new ChessPiece(turn, pn);
            var destination = BoardLocation.At(dest);

            moveQuery = new PgnMoveQuery(piece, destination, moveType, fromFile);
            return true;
        }

        private static Chess.PieceNames GetPieceName(char pieceChar)
        {
            return Chess.PieceNames.Pawn;
        }
    }
}