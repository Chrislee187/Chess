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
                    return !Board.Validations.IsValidLocation(file, rank)
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
                                        && piece.Is(Chess.PieceNames.Pawn)
                                        && piece.IsNot(board[at].Piece.Colour)
                                        && board[takeLocation].MoveHistory.Count() == 1
                        ;
                    var moveToSpotIsVacant = board.IsEmptyAt(moveLocation);

                    return (canTakeAPiece && moveToSpotIsVacant);
                }

                public static int EnpassantFromRankFor(Chess.Colours colour)
                {
                    const int whitePawnsEnPassantFromRank = 5;
                    const int blackPawnsEnPassantFromRank = 4;

                    return colour == Chess.Colours.White
                        ? whitePawnsEnPassantFromRank
                        : colour == Chess.Colours.Black
                            ? blackPawnsEnPassantFromRank : 0;

                }
                
                public static int StartingPawnRankFor(Chess.Colours chessPieceColour)
                {
                    if (chessPieceColour == Chess.Colours.White)
                        return 2;

                    if (chessPieceColour == Chess.Colours.Black)
                        return 7;

                    return 0;
                }

                public static int PromotionRankFor(Chess.Colours chessPieceColour)
                {
                    return chessPieceColour == Chess.Colours.Black
                        ? 1
                        : chessPieceColour == Chess.Colours.White
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