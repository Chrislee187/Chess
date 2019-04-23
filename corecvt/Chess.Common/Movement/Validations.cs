using System;
using System.Linq;
using Chess.Common.Extensions;
using Chess.Common.System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Chess.Common.Movement
{
    // TODO: Unit Tests?
    public static class Validations
    {
        public static bool IsValidLocation(int file, int rank) => !InvalidFile(file) && !InvalidRank(rank);
        public static bool IsValidLocation(BoardLocation boardLocation) => IsValidLocation((int)boardLocation.File, boardLocation.Rank);

        public static bool InvalidFile(ChessFile file) => InvalidFile((int)file);

        public static bool InvalidRank(int rank) => rank < 1 || rank > 8;   
        public static bool InvalidFile(int file) => file < 1 || file > 8;

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

        public static bool IsEmptyAt(Common.Board board, BoardLocation location)
            => board[location].Piece.Equals(ChessPiece.NullPiece);

        public static bool IsNotEmptyAt(Common.Board board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsEmptyAt(Common.Board board, string location)
            => board[(BoardLocation)location].Piece.Equals(ChessPiece.NullPiece);

        public static bool IsNotEmptyAt(Common.Board board, string location)
            => !IsEmptyAt(board, (BoardLocation)location);

        // TODO: Unit Tests?
        public static bool CanCastle(Common.Board board, BoardLocation kingLocation) => CastleLocationsAreEmpty(board, kingLocation);

        public static bool InCheckAt(Common.Board board, BoardLocation at, Colours asPlayer)
        {
            var enemyPieces = board.Pieces.OfColour(Info.ColourOfEnemy(asPlayer));
            var checkPieces = enemyPieces.Where(p => PieceIsAttackingLocation(board, p, at));

            return checkPieces.Any();
        }

        public static bool PieceIsAttackingLocation(Common.Board b, BoardPiece p, BoardLocation l) => b[p.Location].PossibleMoves.Any(m => m.To.Equals(l));
        
        public static bool MovesLeaveOwnSideInCheck(Common.Board board, Move move)
        {
            var moversPiece = board[move.From].Piece;
            var clone = board.ShallowClone();

            if (move.MoveType == MoveType.Castle)
            {
                var locs = King.SquaresKingsPassesThroughWhenCastling(move.To);
                var enemyPieces = clone.Pieces.OfColour(Info.ColourOfEnemy(moversPiece.Colour)).ToList();
                var movesThruCheck = enemyPieces
                    .SelectMany(p => p.PossibleMoves)
                    .Any(moves => locs.Any(l => l.Equals(moves.To)));

                if (movesThruCheck) return true;
            }

            var moversKing = clone.GetKingFor(moversPiece.Colour);
            clone.MovePiece(move);

            return InCheckAt(clone, moversKing.Location, moversPiece.Colour);
        }

        public static bool CastleLocationsAreEmpty(Common.Board board, BoardLocation king)
            => King.SquaresBetweenCastlingPieces(king).All(board.IsEmptyAt);

    }
}
