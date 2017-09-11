using System;
using System.Linq;
using CSharpChess.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace CSharpChess.Movement
{
    // TODO: Unit Tests?
    public static class Validations
    {
        public static bool IsValidLocation(int file, int rank) => !InvalidFile(file) && !InvalidRank(rank);
        public static bool IsValidLocation(BoardLocation boardLocation) => IsValidLocation((int)boardLocation.File, boardLocation.Rank);

        public static bool InvalidRank(int rank) => !Chess.Ranks.Contains(rank);
        public static bool InvalidFile(ChessFile file) => InvalidFile((int)file);
        public static bool InvalidFile(int file) => Chess.Files.All(f => (int)f != file);

        public static void ThrowInvalidRank(int rank)
        {
            if (InvalidRank(rank)) throw new ArgumentOutOfRangeException(nameof(rank), rank, "Invalid Rank");
        }
        public static void ThrowInvalidFile(int file)
        {
            if (InvalidFile(file))
                throw new ArgumentOutOfRangeException(nameof(file), file, "Invalid File");
        }
        public static void ThrowInvalidFile(ChessFile file) => ThrowInvalidFile((int)file);

        public static bool IsEmptyAt(CSharpChess.Board board, BoardLocation location)
            => board[location].Piece.Equals(ChessPiece.NullPiece);

        public static bool IsNotEmptyAt(CSharpChess.Board board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsEmptyAt(CSharpChess.Board board, string location)
            => board[(BoardLocation)location].Piece.Equals(ChessPiece.NullPiece);

        public static bool IsNotEmptyAt(CSharpChess.Board board, string location)
            => !IsEmptyAt(board, (BoardLocation)location);

        // TODO: Unit Tests?
        public static bool CanCastle(CSharpChess.Board board, BoardLocation kingLocation) => CastleLocationsAreEmpty(board, kingLocation);

        public static bool InCheckAt(CSharpChess.Board board, BoardLocation at, Colours asPlayer)
        {
            var enemyPieces = board.Pieces.OfColour(Chess.ColourOfEnemy(asPlayer));
            var checkPieces = enemyPieces.Where(p => PieceIsAttackingLocation(board, p, at));

            return checkPieces.Any();
        }

        public static bool PieceIsAttackingLocation(CSharpChess.Board b, BoardPiece p, BoardLocation l) => b[p.Location].PossibleMoves.Any(m => m.To.Equals(l));
        
        public static bool MovesLeaveOwnSideInCheck(CSharpChess.Board board, Move move)
        {
            var moversPiece = board[move.From].Piece;
            var clone = board.ShallowClone();

            if (move.MoveType == MoveType.Castle)
            {
                var locs = King.SquaresKingsPassesThroughWhenCastling(move.To);
                var enemyPieces = clone.Pieces.OfColour(Chess.ColourOfEnemy(moversPiece.Colour)).ToList();
                var movesThruCheck = enemyPieces
                    .SelectMany(p => p.PossibleMoves)
                    .Any(moves => locs.Any(l => l.Equals(moves.To)));

                if (movesThruCheck) return true;
            }

            var moversKing = clone.GetKingFor(moversPiece.Colour);
            clone.MovePiece(move);

            return InCheckAt(clone, moversKing.Location, moversPiece.Colour);
        }

        public static bool CastleLocationsAreEmpty(CSharpChess.Board board, BoardLocation king)
            => King.SquaresBetweenCastlingPieces(king).All(board.IsEmptyAt);

    }
}
