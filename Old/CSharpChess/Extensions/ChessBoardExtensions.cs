using System.Collections.Generic;
using System.Linq;
using CSharpChess.Movement;
using CSharpChess.System;

namespace CSharpChess.Extensions
{
    public static class ChessBoardExtensions
    {
        public static bool GameOver(this Board board)
        {
            var gameOverStates = new[] {GameState.CheckMateBlackWins, GameState.CheckMateWhiteWins, GameState.Stalemate};
            return gameOverStates.Any(s => s == board.GameState);
        }
        public static IEnumerable<IEnumerable<BoardPiece>> Ranks(this Board board)
        {
            foreach (var rank in Info.Ranks)
            {
                var list = new List<BoardPiece>();
                foreach (var chessFile in Info.Files)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }
        public static IEnumerable<IEnumerable<BoardPiece>> Files(this Board board)
        {
            foreach (var chessFile in Info.Files)
            {
                var list = new List<BoardPiece>();
                foreach (var rank in Info.Ranks)
                {
                    list.Add(board[BoardLocation.At(chessFile, rank)]);
                }
                yield return list;
            }
        }

        public static bool CanCastle(this Board board, BoardLocation destination) => 
            Validations.CanCastle(board, destination);

        public static bool InCheckAt(this Board board, BoardLocation at, Colours asPlayer)
            => Validations.InCheckAt(board, at, asPlayer);

        public static bool IsEmptyAt(this Board board, BoardLocation location)
            => Validations.IsEmptyAt(board, location);

        public static bool MovesLeaveOwnSideInCheck(this Board board, Move move)
            => Validations.MovesLeaveOwnSideInCheck(board, move);

        public static bool IsEmptyAt(this Board board, string location)
            => IsEmptyAt(board, BoardLocation.At(location));

        public static bool IsNotEmptyAt(this Board board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsNotEmptyAt(this Board board, string location)
            => !IsEmptyAt(board, BoardLocation.At(location));

        public static bool CanTakeAt(this Board board, BoardLocation takeLocation, Colours attackerColour)
            => IsNotEmptyAt(board, takeLocation)
               && board[takeLocation].Piece.Is(Info.ColourOfEnemy(attackerColour));

        public static bool IsCoveringAt(this Board board, BoardLocation coverLocation, Colours attackerColour)
            => IsNotEmptyAt(board, coverLocation)
               && board[coverLocation].Piece.Is(attackerColour);

        public static BoardPiece GetKingFor(this Board board, Colours colour) 
            => board.Pieces.FirstOrDefault(p => p.Piece.Is(colour, PieceNames.King));
    }
}