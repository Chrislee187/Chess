using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using static CSharpChess.Chess.Board.LocationMover;

namespace CSharpChess
{
    public static partial class Chess
    {
        public static class Rules
        {
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
                public static readonly IEnumerable<Board.LocationMover> MovementTransformations = new List<Board.LocationMover>
                {
                    Create(Moves.Right(1), Moves.Up(2)),
                    Create(Moves.Right(2), Moves.Up(1)),
                    Create(Moves.Right(2), Moves.Down(1)),
                    Create(Moves.Right(1), Moves.Down(2)),
                    Create(Moves.Left(1), Moves.Down(2)),
                    Create(Moves.Left(2), Moves.Down(1)),
                    Create(Moves.Left(2), Moves.Up (1)),
                    Create(Moves.Left(1), Moves.Up(2))
                };
            }
            public static class Bishops
            {
                public static IEnumerable<Board.LocationMover> MovementTransformations => new List<Board.LocationMover>
                {
                    Create(Moves.Right(1), Moves.Up(1)),
                    Create(Moves.Right(1), Moves.Down(1)),
                    Create(Moves.Left(1), Moves.Up(1)),
                    Create(Moves.Left(1), Moves.Down(1))
                };
            }
            public static class Rooks
            {
                public static IEnumerable<Board.LocationMover> MovementTransformations => new List<Board.LocationMover>
                {
                    Create(Moves.None(), Moves.Up(1)),
                    Create(Moves.Right(1), Moves.None()),
                    Create(Moves.None(), Moves.Down(1)),
                    Create(Moves.Left(1), Moves.None())
                };
            }
            public static class Queen
            {
                public static IEnumerable<Board.LocationMover> MovementTransformations
                    => Bishops.MovementTransformations.Concat(Rooks.MovementTransformations);
            }
            public static class King
            {
                public static IEnumerable<Board.LocationMover> MovementTransformations
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