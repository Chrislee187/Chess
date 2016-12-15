using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.ValidMoves;

namespace CSharpChess
{
    public static class Chess
    {

        public enum GameState
        {
            BlackKingInCheck, WhiteKingInCheck, WaitingForMove,
            Unknown
        }
        public static class Board
        {
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
                Func<ChessBoard, BoardPiece, BoardLocation, bool> pieceIsAttackingLocation =
                    delegate (ChessBoard b, BoardPiece p, BoardLocation l)
                    {
                        return b[p.Location].AllMoves
                            .Any(m => m.To.Equals(l));
                    };
                var checkPieces = enemyPieces.Where(p => pieceIsAttackingLocation(board, p, at));

                return checkPieces.Any();
            }

            public static bool MoveDoesNotPutOwnKingInCheck(ChessBoard board, ChessMove move)
            {
                var clone = board.ShallowClone();
                var moversPiece = board[move.From].Piece;
                var moversKing = clone.GetKingFor(moversPiece.Colour);
                clone.MovePiece(move);
                clone.MoveHandler.RebuildMoveLists();
                return !InCheckAt(clone, moversKing.Location, moversPiece.Colour);
            }

            public static bool CastleLocationsAreEmpty(ChessBoard board, BoardLocation king, BoardLocation rook)
            {
                var mustBeEmpty = CastleLocationsBetween(king, rook);

                return mustBeEmpty.All(board.IsEmptyAt);
            }

            public static IEnumerable<BoardLocation> CastleLocationsBetween(BoardLocation fromLoc, BoardLocation toLoc)
            {
                int fromFile, toFile;
                if (toLoc.File == ChessFile.C)
                {
                    fromFile = (int) ChessFile.C;
                    toFile = (int) ChessFile.D;
                }
                else
                {
                    fromFile = (int) ChessFile.F;
                    toFile = (int) ChessFile.G;
                }

                return Enumerable.Range(fromFile, toFile - fromFile + 1).Select(v => BoardLocation.At(v, fromLoc.Rank));
            }


            public static bool IsEmptyAt(ChessBoard board, BoardLocation location)
                => board[location].Piece.Equals(ChessPiece.NullPiece);

            public static bool IsNotEmptyAt(ChessBoard board, BoardLocation location)
                => !IsEmptyAt(board, location);

            public static bool IsEmptyAt(ChessBoard board, string location)
                => board[(BoardLocation)location].Piece.Equals(ChessPiece.NullPiece);

            public static bool IsNotEmptyAt(ChessBoard board, string location)
                => !IsEmptyAt(board, (BoardLocation)location);


            public enum Colours { White, Black, None = -9999 }

            public enum PieceNames { Pawn, Rook, Bishop, Knight, King, Queen, Blank = -9999 }

            public enum ChessFile { A = 1, B, C, D, E, F, G, H };
            public static IEnumerable<ChessFile> Files => EnumExtensions.GetAll<ChessFile>();

            public static IEnumerable<int> Ranks => Enumerable.Range(1, 8);

            // These need to be an enum
            public const int LeftDirectionModifier = -1;
            public const int RightDirectionModifier = 1;

            public static bool IsValidLocation(int file, int rank)
            {
                return !Validations.InvalidFile(file)
                    && !Validations.InvalidRank(rank);
            }
            public static bool IsValidLocation(BoardLocation boardLocation)
            {
                return IsValidLocation((int)boardLocation.File, boardLocation.Rank);
            }

            public static int ForwardDirectionModifierFor(ChessPiece piece)
            {
                return piece.Colour == Colours.White
                    ? +1
                    : piece.Colour == Colours.Black
                        ? -1 : 0;

            }

        }

        public static Board.Colours ColourOfEnemy(Board.Colours colour) => colour == Board.Colours.Black
            ? Board.Colours.White
            : colour == Board.Colours.White
                ? Board.Colours.Black
                : colour;

        public static class Pieces
        {
            public static readonly ChessPiece Blank = ChessPiece.NullPiece;

            public static class White
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Board.Colours.White, Board.PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Board.Colours.White, Board.PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Board.Colours.White, Board.PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Board.Colours.White, Board.PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Board.Colours.White, Board.PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Board.Colours.White, Board.PieceNames.Queen);
            }
            public static class Black
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Board.Colours.Black, Board.PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Board.Colours.Black, Board.PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Board.Colours.Black, Board.PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Board.Colours.Black, Board.PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Board.Colours.Black, Board.PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Board.Colours.Black, Board.PieceNames.Queen);
            }
        }

        public static class Validations
        {
            public static bool InvalidRank(int rank) => !Board.Ranks.Contains(rank);

            public static bool InvalidFile(Board.ChessFile file) => InvalidFile((int)file);
            public static bool InvalidFile(int file) => Board.Files.All(f => (int)f != file);

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

            public static void ThrowInvalidFile(Board.ChessFile file)
            {
                ThrowInvalidFile((int)file);
            }
        }

        public static class Rules
        {
            public class MovementTransformation
            {
                public static MovementTransformation Create(int x, int y) => new MovementTransformation(x, y);

                public static IEnumerable<BoardLocation> ApplyTo(BoardLocation loc, IEnumerable<MovementTransformation> movements)
                    => movements.Select(m => m.ApplyTo(loc)).Where(l => l != null);

                internal delegate int Transform(int i);
                private readonly int _transformX;
                private readonly int _transformY;

                internal static class Movement
                {
                    internal static Transform Left => (i) => -1 * i;
                    internal static Transform Right => (i) => +1 * i;
                    internal static Transform Down => (i) => -1 * i;
                    internal static Transform Up => (i) => +1 * i;
                    internal static Func<int> None => () => 0;
                }

                public BoardLocation ApplyTo(BoardLocation from)
                {
                    var file = (int)@from.File + _transformX;
                    var rank = @from.Rank + _transformY;
                    return !Board.IsValidLocation(file, rank)
                        ? null
                        : new BoardLocation((Board.ChessFile)file, rank);
                }

                private MovementTransformation(int transformX, int transformY)
                {
                    _transformX = transformX;
                    _transformY = transformY;
                }
            }

            public static class Pawns
            {
                // TODO: Unit Test this
                public static bool CanEnPassant(ChessBoard board, BoardLocation at, BoardLocation moveLocation)
                {
                    var newFile = moveLocation.File;
                    var takeLocation = new BoardLocation(newFile, at.Rank);
                    var piece = board[takeLocation].Piece;
                    var canTakeAPiece = board.IsNotEmptyAt(takeLocation)
                                        && piece.Is(Board.PieceNames.Pawn)
                                        && piece.IsNot(board[at].Piece.Colour)
                                        && board[takeLocation].MoveHistory.Count() == 1
                        ;
                    var moveToSpotIsVacant = board.IsEmptyAt(moveLocation);

                    return (canTakeAPiece && moveToSpotIsVacant);
                }

                public static int EnpassantFromRankFor(Board.Colours colour)
                {
                    const int whitePawnsEnPassantFromRank = 5;
                    const int blackPawnsEnPassantFromRank = 4;

                    return colour == Board.Colours.White
                        ? whitePawnsEnPassantFromRank
                        : colour == Board.Colours.Black
                            ? blackPawnsEnPassantFromRank : 0;

                }
                
                public static int StartingPawnRankFor(Board.Colours chessPieceColour)
                {
                    if (chessPieceColour == Board.Colours.White)
                        return 2;

                    if (chessPieceColour == Board.Colours.Black)
                        return 7;

                    return 0;
                }

                public static int PromotionRankFor(Board.Colours chessPieceColour)
                {
                    return chessPieceColour == Board.Colours.Black
                        ? 1
                        : chessPieceColour == Board.Colours.White
                            ? 8
                            : 0;
                }
            }
            public static class Knights
            {
                public static readonly IEnumerable<MovementTransformation> MoveMatrix = new List<MovementTransformation>
                {
                    MovementTransformation.Create(MovementTransformation.Movement.Right(1), MovementTransformation.Movement.Up(2)),
                    MovementTransformation.Create(MovementTransformation.Movement.Right(2), MovementTransformation.Movement.Up(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Right(2), MovementTransformation.Movement.Down(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Right(1), MovementTransformation.Movement.Down(2)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(1), MovementTransformation.Movement.Down(2)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(2), MovementTransformation.Movement.Down(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(2), MovementTransformation.Movement.Up (1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(1), MovementTransformation.Movement.Up(2))
                };
            }
            public static class Bishops
            {
                public static IEnumerable<MovementTransformation> DirectionTransformations => new List<MovementTransformation>
                {
                    MovementTransformation.Create(MovementTransformation.Movement.Right(1), MovementTransformation.Movement.Up(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Right(1), MovementTransformation.Movement.Down(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(1), MovementTransformation.Movement.Up(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(1), MovementTransformation.Movement.Down(1))
                };
            }
            public static class Rooks
            {
                public static IEnumerable<MovementTransformation> DirectionTransformations => new List<MovementTransformation>
                {
                    MovementTransformation.Create(MovementTransformation.Movement.None(), MovementTransformation.Movement.Up(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Right(1), MovementTransformation.Movement.None()),
                    MovementTransformation.Create(MovementTransformation.Movement.None(), MovementTransformation.Movement.Down(1)),
                    MovementTransformation.Create(MovementTransformation.Movement.Left(1), MovementTransformation.Movement.None())
                };
            }
            public static class KingAndQueen
            {
                public static IEnumerable<MovementTransformation> DirectionTransformations
                    => Bishops.DirectionTransformations.Concat(Rooks.DirectionTransformations);

                public static ChessMove CreateRookMoveForCastling(ChessMove move)
                {
                    BoardLocation rook;
                    BoardLocation rookTo;
                    if (move.To.File == Board.ChessFile.C)
                    {
                        rook = BoardLocation.At(Board.ChessFile.A, move.From.Rank);
                        rookTo = BoardLocation.At(Board.ChessFile.D, move.From.Rank);
                    }
                    else
                    {
                        rook = BoardLocation.At(Board.ChessFile.H, move.From.Rank);
                        rookTo = BoardLocation.At(Board.ChessFile.F, move.From.Rank);
                    }

                    return new ChessMove(rook, rookTo, MoveType.Castle);
                }
            }
        }
    }
}