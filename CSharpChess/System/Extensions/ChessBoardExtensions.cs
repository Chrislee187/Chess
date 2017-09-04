using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using static CSharpChess.Chess;

namespace CSharpChess.System.Extensions
{
    public static class ChessBoardExtensions
    {
        public static bool GameOver(this ChessBoard board)
        {
            var gameOverStates = new[] {GameState.CheckMateBlackWins, GameState.CheckMateWhiteWins, GameState.Stalemate};
            return gameOverStates.Any(s => s == board.GameState);
        }
        public static IEnumerable<IEnumerable<BoardPiece>> Ranks(this ChessBoard board)
        {
            foreach (var rank in Chess.Ranks)
            {
                var list = new List<BoardPiece>();
                foreach (var chessFile in Chess.Files)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }
        public static IEnumerable<IEnumerable<BoardPiece>> Files(this ChessBoard board)
        {
            foreach (var chessFile in Chess.Files)
            {
                var list = new List<BoardPiece>();
                foreach (var rank in Chess.Ranks)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }

        public static bool CanCastle(this ChessBoard board, BoardLocation destination) => 
            Validations.CanCastle(board, destination);

        public static bool InCheckAt(this ChessBoard board, BoardLocation at, Colours asPlayer)
            => Validations.InCheckAt(board, at, asPlayer);

        public static bool IsEmptyAt(this ChessBoard board, BoardLocation location)
            => Validations.IsEmptyAt(board, location);

        public static bool MovesLeaveOwnSideInCheck(this ChessBoard board, ChessMove move)
            => Validations.MovesLeaveOwnSideInCheck(board, move);

        public static bool IsEmptyAt(this ChessBoard board, string location)
            => IsEmptyAt(board, BoardLocation.At(location));

        public static bool IsNotEmptyAt(this ChessBoard board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsNotEmptyAt(this ChessBoard board, string location)
            => !IsEmptyAt(board, BoardLocation.At(location));

        public static bool CanTakeAt(this ChessBoard board, BoardLocation takeLocation, Colours attackerColour)
            => IsNotEmptyAt(board, takeLocation)
               && board[takeLocation].Piece.Is(ColourOfEnemy(attackerColour));

        public static bool IsCoveringAt(this ChessBoard board, BoardLocation coverLocation, Colours attackerColour)
            => IsNotEmptyAt(board, coverLocation)
               && board[coverLocation].Piece.Is(attackerColour);

        public static BoardPiece GetKingFor(this ChessBoard board, Colours colour) 
            => board.Pieces.FirstOrDefault(p => p.Piece.Is(colour, PieceNames.King));
    }
}