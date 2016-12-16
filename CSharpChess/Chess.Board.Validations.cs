using System;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace CSharpChess
{
    public static partial class Chess
    {
        public static partial class Board
        {
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

                public static ChessMove CanCastle(ChessBoard board, BoardLocation kingLocation, BoardLocation rookLoc)
                {
                    var rookPiece = board[rookLoc];
                    if (rookPiece.Piece.IsNot(PieceNames.Rook) || rookPiece.MoveHistory.Any()) return null;

                    var kingPiece = board[kingLocation];
                    if (kingPiece.Piece.IsNot(PieceNames.King) || kingPiece.MoveHistory.Any()) return null;

                    if (!CastleLocationsAreEmpty(board, kingLocation, rookPiece.Location)) return null;

                    var castleFile = rookPiece.Location.File == ChessFile.A ? ChessFile.C : ChessFile.G;
                    return new ChessMove(kingLocation, BoardLocation.At(castleFile, kingLocation.Rank), MoveType.Castle);
                }

                public static bool InCheckAt(ChessBoard board, BoardLocation at, Colours asPlayer)
                {
                    var enemyPieces = board.Pieces.OfColour(ColourOfEnemy(asPlayer));
                    var checkPieces = enemyPieces.Where(p => PieceIsAttackingLocation(board, p, at));

                    return checkPieces.Any();
                }

                public static bool PieceIsAttackingLocation(ChessBoard b, BoardPiece p, BoardLocation l)
                {
                    return b[p.Location].PossibleMoves.Any(m => m.To.Equals(l));
                }

                public static bool MovesLeaveOwnSideInCheck(ChessBoard board, ChessMove move)
                {
                    var moversPiece = board[move.From].Piece;
                    var clone = board.ShallowClone();

                    if (move.MoveType == MoveType.Castle)
                    {
                        var locs = Rules.King.CastleLocationsBetween(move.From, move.To);
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

                public static bool CastleLocationsAreEmpty(ChessBoard board, BoardLocation king, BoardLocation rook)
                    => Rules.King.CastleLocationsBetween(king, rook).All(board.IsEmptyAt);

            }
        }
    }
}