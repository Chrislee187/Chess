using System;
using System.Linq;
using CSharpChess.Rules;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using static CSharpChess.Chess;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace CSharpChess.TheBoard
{
    // TODO: Unit Tests?
    public static class Validations
    {
        public static bool IsValidLocation(int file, int rank) => !InvalidFile(file) && !InvalidRank(rank);
        public static bool IsValidLocation(BoardLocation boardLocation) => IsValidLocation((int)boardLocation.File, boardLocation.Rank);

        public static bool InvalidRank(int rank) => !Ranks.Contains(rank);
        public static bool InvalidFile(ChessFile file) => InvalidFile((int)file);
        public static bool InvalidFile(int file) => Files.All(f => (int)f != file);

        public static void ThrowInvalidRank(int rank)
        {
            if (InvalidRank(rank))
                throw new ArgumentOutOfRangeException(nameof(rank), rank, "Invalid Rank");
        }
        public static void ThrowInvalidFile(int file)
        {
            if (InvalidFile(file))
                throw new ArgumentOutOfRangeException(nameof(file), file, "Invalid File");
        }
        public static void ThrowInvalidFile(ChessFile file)
        {
            ThrowInvalidFile((int)file);
        }

        public static bool IsEmptyAt(ChessBoard board, BoardLocation location)
            => board[location].Piece.Equals(ChessPiece.NullPiece);

        public static bool IsNotEmptyAt(ChessBoard board, BoardLocation location)
            => !IsEmptyAt(board, location);

        public static bool IsEmptyAt(ChessBoard board, string location)
            => board[(BoardLocation)location].Piece.Equals(ChessPiece.NullPiece);

        public static bool IsNotEmptyAt(ChessBoard board, string location)
            => !IsEmptyAt(board, (BoardLocation)location);

        // TODO: Unit Tests?
        public static bool CanCastle(ChessBoard board, BoardLocation kingLocation)
        {
            return CastleLocationsAreEmpty(board, kingLocation);

        }

        public static bool InCheckAt(ChessBoard board, BoardLocation at, Colours asPlayer)
        {
            var enemyPieces = board.Pieces.OfColour(Chess.ColourOfEnemy(asPlayer));
            var checkPieces = enemyPieces.Where(p => PieceIsAttackingLocation(board, p, at));

            return checkPieces.Any();
        }

        public static bool PieceIsAttackingLocation(ChessBoard b, BoardPiece p, BoardLocation l)
        {
            return b[p.Location].PossibleMoves.Any(m => m.To.Equals(l));
        }
        // TODO: Unit Test this and everything else in here
        public static bool MovesLeaveOwnSideInCheck(ChessBoard board, ChessMove move)
        {
            var moversPiece = board[move.From].Piece;
            var clone = board.ShallowClone();

            if (move.MoveType == MoveType.Castle)
            {
                var locs = King.SquaresKingsPassesThroughWhenCastling(move.To);
                var boardPieces = clone.Pieces.OfColour(ColourOfEnemy(moversPiece.Colour)).ToList();
                var movesThruCheck = boardPieces
                    .SelectMany(p => p.PossibleMoves)
                    .Any(moves => locs.Any(l => l.Equals(moves.To)));

                if (movesThruCheck) return true;
            }

            var moversKing = clone.GetKingFor(moversPiece.Colour);
            clone.MovePiece(move);

            return InCheckAt(clone, moversKing.Location, moversPiece.Colour);
        }

        public static bool CastleLocationsAreEmpty(ChessBoard board, BoardLocation king)
            => King.SquaresBetweenCastlingPieces(king).All(board.IsEmptyAt);

    }
}
