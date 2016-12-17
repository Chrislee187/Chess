using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public static partial class Chess
    {
        public static class Rules
        {
            public class TransformLocation
            {
                public static TransformLocation Create(int x, int y) => new TransformLocation(x, y);

                public static IEnumerable<BoardLocation> MoveWhile(ChessBoard board, BoardLocation from,
                    TransformLocation transformation, Func<ChessBoard, BoardLocation, bool> @while)
                {
                    var result = new List<BoardLocation>();

                    var to = transformation.ApplyTo(from);

                    while (to != null && @while(board, to))
                    {
                        result.Add(to);
                        to = transformation.ApplyTo(to);
                    }
                    return result;
                }

                public static IEnumerable<BoardLocation> ApplyManyTo(BoardLocation loc, IEnumerable<TransformLocation> movements)
                    => movements.Select(m => m.ApplyTo(loc)).Where(l => l != null);

                public BoardLocation ApplyTo(BoardLocation from)
                {
                    var file = (int)from.File + _transformX;
                    var rank = from.Rank + _transformY;
                    return !Board.Validations.IsValidLocation(file, rank)
                        ? null
                        : new BoardLocation((Board.ChessFile)file, rank);
                }

                private readonly int _transformX;
                private readonly int _transformY;

                internal static class Move
                {
                    internal delegate int Transform(int i);
                    internal static Transform Left => (i) => -1 * i;
                    internal static Transform Right => (i) => +1 * i;
                    internal static Transform Down => (i) => -1 * i;
                    internal static Transform Up => (i) => +1 * i;
                    internal static Func<int> None => () => 0;
                }

                private TransformLocation(int transformX, int transformY)
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
                                        && piece.Is(PieceNames.Pawn)
                                        && piece.IsNot(board[at].Piece.Colour)
                                        && board[takeLocation].MoveHistory.Count() == 1
                        ;
                    var moveToSpotIsVacant = board.IsEmptyAt(moveLocation);

                    return (canTakeAPiece && moveToSpotIsVacant);
                }

                public static int EnpassantFromRankFor(Colours colour)
                {
                    const int whitePawnsEnPassantFromRank = 5;
                    const int blackPawnsEnPassantFromRank = 4;

                    return colour == Colours.White
                        ? whitePawnsEnPassantFromRank
                        : colour == Colours.Black
                            ? blackPawnsEnPassantFromRank : 0;

                }
                
                public static int StartingPawnRankFor(Colours chessPieceColour)
                {
                    if (chessPieceColour == Colours.White)
                        return 2;

                    if (chessPieceColour == Colours.Black)
                        return 7;

                    return 0;
                }

                public static int PromotionRankFor(Colours chessPieceColour)
                {
                    return chessPieceColour == Colours.Black
                        ? 1
                        : chessPieceColour == Colours.White
                            ? 8
                            : 0;
                }
            }
            public static class Knights
            {
                public static readonly IEnumerable<TransformLocation> MovementTransformations = new List<TransformLocation>
                {
                    TransformLocation.Create(TransformLocation.Move.Right(1), TransformLocation.Move.Up(2)),
                    TransformLocation.Create(TransformLocation.Move.Right(2), TransformLocation.Move.Up(1)),
                    TransformLocation.Create(TransformLocation.Move.Right(2), TransformLocation.Move.Down(1)),
                    TransformLocation.Create(TransformLocation.Move.Right(1), TransformLocation.Move.Down(2)),
                    TransformLocation.Create(TransformLocation.Move.Left(1), TransformLocation.Move.Down(2)),
                    TransformLocation.Create(TransformLocation.Move.Left(2), TransformLocation.Move.Down(1)),
                    TransformLocation.Create(TransformLocation.Move.Left(2), TransformLocation.Move.Up (1)),
                    TransformLocation.Create(TransformLocation.Move.Left(1), TransformLocation.Move.Up(2))
                };
            }
            public static class Bishops
            {
                public static IEnumerable<TransformLocation> MovementTransformations => new List<TransformLocation>
                {
                    TransformLocation.Create(TransformLocation.Move.Right(1), TransformLocation.Move.Up(1)),
                    TransformLocation.Create(TransformLocation.Move.Right(1), TransformLocation.Move.Down(1)),
                    TransformLocation.Create(TransformLocation.Move.Left(1), TransformLocation.Move.Up(1)),
                    TransformLocation.Create(TransformLocation.Move.Left(1), TransformLocation.Move.Down(1))
                };
            }
            public static class Rooks
            {
                public static IEnumerable<TransformLocation> MovementTransformations => new List<TransformLocation>
                {
                    TransformLocation.Create(TransformLocation.Move.None(), TransformLocation.Move.Up(1)),
                    TransformLocation.Create(TransformLocation.Move.Right(1), TransformLocation.Move.None()),
                    TransformLocation.Create(TransformLocation.Move.None(), TransformLocation.Move.Down(1)),
                    TransformLocation.Create(TransformLocation.Move.Left(1), TransformLocation.Move.None())
                };
            }
            public static class Queen
            {
                public static IEnumerable<TransformLocation> MovementTransformations
                    => Bishops.MovementTransformations.Concat(Rooks.MovementTransformations);
            }
            public static class King
            {
                public static IEnumerable<TransformLocation> MovementTransformations
                    => Queen.MovementTransformations;

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

                public static IEnumerable<BoardLocation> CastleLocationsBetween(BoardLocation fromLoc, BoardLocation toLoc)
                {
                    int fromFile, toFile;
                    if (toLoc.File == Board.ChessFile.C)
                    {
                        fromFile = (int)Board.ChessFile.C;
                        toFile = (int)Board.ChessFile.D;
                    }
                    else
                    {
                        fromFile = (int)Board.ChessFile.F;
                        toFile = (int)Board.ChessFile.G;
                    }

                    return Enumerable.Range(fromFile, toFile - fromFile + 1).Select(v => BoardLocation.At(v, fromLoc.Rank));
                }


            }

        }
    }
}