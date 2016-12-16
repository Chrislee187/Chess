using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.System.Extensions
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
            Chess.Board.Validations.CanCastle(board, at, rookLoc);

        public static bool InCheckAt(this ChessBoard board, BoardLocation at, Chess.Colours asPlayer)
            => Chess.Board.Validations.InCheckAt(board, at, asPlayer);

        public static bool IsEmptyAt(this ChessBoard board, BoardLocation location)
            => Chess.Board.Validations.IsEmptyAt(board, location);

        public static bool MovesLeaveOwnSideInCheck(this ChessBoard board, ChessMove move)
            => Chess.Board.Validations.MovesLeaveOwnSideInCheck(board, move);

        public static bool IsEmptyAt(this ChessBoard board, string location)
            => IsEmptyAt(board, BoardLocation.At(location));

        public static bool IsNotEmptyAt(this ChessBoard board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsNotEmptyAt(this ChessBoard board, string location)
            => !IsEmptyAt(board, BoardLocation.At(location));

        public static bool CanTakeAt(this ChessBoard board, BoardLocation takeLocation, Chess.Colours attackerColour)
            => IsNotEmptyAt(board, takeLocation)
               && board[takeLocation].Piece.Is(Chess.ColourOfEnemy(attackerColour));

        public static bool IsCoveringAt(this ChessBoard board, BoardLocation coverLocation, Chess.Colours attackerColour)
            => IsNotEmptyAt(board, coverLocation)
               && board[coverLocation].Piece.Is(attackerColour);

        public static BoardPiece GetKingFor(this ChessBoard board, Chess.Colours colour) 
            => board.Pieces.FirstOrDefault(p => p.Piece.Is(colour, Chess.PieceNames.King));
    }
}