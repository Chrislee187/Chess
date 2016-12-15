using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class ChessBoardExtensions
    {
        public static IEnumerable<IEnumerable<BoardPiece>> Ranks(this ChessBoard board)
        {
            foreach (var rank in Chess.Board.Ranks)
            {
                var list = new List<BoardPiece>();
                foreach (var chessFile in Chess.Board.Files)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }
        public static IEnumerable<IEnumerable<BoardPiece>> Files(this ChessBoard board)
        {
            foreach (var chessFile in Chess.Board.Files)
            {
                var list = new List<BoardPiece>();
                foreach (var rank in Chess.Board.Ranks)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }

        public static ChessMove CanCastle(this ChessBoard board, BoardLocation at, BoardLocation rookLoc) => 
            Chess.Board.CanCastle(board, at, rookLoc);

        public static bool InCheckAt(this ChessBoard board, BoardLocation at, Chess.Board.Colours asPlayer)
            => Chess.Board.InCheckAt(board, at, asPlayer);

        public static bool IsEmptyAt(this ChessBoard board, BoardLocation location)
            => Chess.Board.IsEmptyAt(board, location);

        public static bool IsEmptyAt(this ChessBoard board, string location)
            => IsEmptyAt(board, BoardLocation.At(location));

        public static bool IsNotEmptyAt(this ChessBoard board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsNotEmptyAt(this ChessBoard board, string location)
            => !IsEmptyAt(board, BoardLocation.At(location));

        public static bool CanTakeAt(this ChessBoard board, BoardLocation takeLocation, Chess.Board.Colours attackerColour)
            => IsNotEmptyAt(board, takeLocation)
               && board[takeLocation].Piece.Colour == Chess.ColourOfEnemy(attackerColour);

        public static bool IsCoveringAt(this ChessBoard board, BoardLocation coverLocation, Chess.Board.Colours attackerColour)
            => IsNotEmptyAt(board, coverLocation)
               && board[coverLocation].Piece.Colour == attackerColour;

        public static bool MoveDoesNotPutOwnKingInCheck(this ChessBoard board, ChessMove move) 
            => Chess.Board.MoveDoesNotPutOwnKingInCheck(board, move);

        public static BoardPiece GetKingFor(this ChessBoard board, Chess.Board.Colours colour) 
            => board.Pieces.First(p => p.Piece.Is(colour, Chess.Board.PieceNames.King));
    }
}