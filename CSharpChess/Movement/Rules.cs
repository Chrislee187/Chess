using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.System;

namespace CSharpChess.Movement
{
    public static class Board
    {
        public enum DirectionModifiers
        {
            LeftDirectionModifier = -1,
            RightDirectionModifier = 1,
            UpBoardDirectionModifer = 1,
            DownBoardDirectionModifer = -1,
            NoDirectionModifier = 0
        }

        // TODO: Unit Test these
        public static int ForwardDirectionModifierFor(ChessPiece piece)
        {
            return (int)(piece.Colour == Colours.White
                ? DirectionModifiers.UpBoardDirectionModifer
                : piece.Colour == Colours.Black
                    ? DirectionModifiers.DownBoardDirectionModifer : DirectionModifiers.NoDirectionModifier);

        }

        public static bool NotOnEdge(BoardLocation at, DirectionModifiers horizontal)
        {
            var notOnHorizontalEdge = horizontal > 0
                ? at.File < ChessFile.H
                : at.File > ChessFile.A;
            return notOnHorizontalEdge;
        }
    }

    public static class Bishops
    {
        public static IEnumerable<LocationFactory> MovementTransformations => new List<LocationFactory>
            {
                LocationFactory.Create(LocationFactory.Moves.Right(1), LocationFactory.Moves.Up(1)),
                LocationFactory.Create(LocationFactory.Moves.Right(1), LocationFactory.Moves.Down(1)),
                LocationFactory.Create(LocationFactory.Moves.Left(1), LocationFactory.Moves.Up(1)),
                LocationFactory.Create(LocationFactory.Moves.Left(1), LocationFactory.Moves.Down(1))
            };
    }
    public static class Rooks
    {
        public static IEnumerable<LocationFactory> MovementTransformations => new List<LocationFactory>
            {
                LocationFactory.Create(LocationFactory.Moves.None(), LocationFactory.Moves.Up(1)),
                LocationFactory.Create(LocationFactory.Moves.Right(1), LocationFactory.Moves.None()),
                LocationFactory.Create(LocationFactory.Moves.None(), LocationFactory.Moves.Down(1)),
                LocationFactory.Create(LocationFactory.Moves.Left(1), LocationFactory.Moves.None())
            };
    }
    public static class Queen
    {
        public static IEnumerable<LocationFactory> MovementTransformations
            => Bishops.MovementTransformations.Concat(Rooks.MovementTransformations);
    }
    public static class King
    {
        public static IEnumerable<LocationFactory> MovementTransformations
            => Queen.MovementTransformations;

        public static Move CreateRookMoveForCastling(Move move)
        {
            BoardLocation rook;
            BoardLocation rookTo;
            if (move.To.File == ChessFile.C)
            {
                rook = BoardLocation.At(ChessFile.A, move.From.Rank);
                rookTo = BoardLocation.At(ChessFile.D, move.From.Rank);
            }
            else
            {
                rook = BoardLocation.At(ChessFile.H, move.From.Rank);
                rookTo = BoardLocation.At(ChessFile.F, move.From.Rank);
            }

            return new Move(rook, rookTo, MoveType.Castle);
        }

        public static IEnumerable<BoardLocation> SquaresBetweenCastlingPieces(BoardLocation toLoc)
        {
            if (toLoc.File == ChessFile.C)
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At(ChessFile.B, toLoc.Rank),
                        BoardLocation.At(ChessFile.C, toLoc.Rank),
                        BoardLocation.At(ChessFile.D, toLoc.Rank),
                    };
            }
            else
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At(ChessFile.F, toLoc.Rank),
                        BoardLocation.At(ChessFile.G, toLoc.Rank),
                    };
            }

            //                    return Enumerable.Range(fromFile, toFile - fromFile + 1).Select(v => BoardLocation.At(v, fromLoc.Rank));
        }

        public static IEnumerable<BoardLocation> SquaresKingsPassesThroughWhenCastling(BoardLocation toLoc)
        {
            if (toLoc.File == ChessFile.C)
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At(ChessFile.D, toLoc.Rank),
                        BoardLocation.At(ChessFile.C, toLoc.Rank)
                    };
            }
            else
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At(ChessFile.F, toLoc.Rank),
                        BoardLocation.At(ChessFile.G, toLoc.Rank),
                    };
            }

            //                    return Enumerable.Range(fromFile, toFile - fromFile + 1).Select(v => BoardLocation.At(v, fromLoc.Rank));
        }

    }

    public static class Knights
    {
        public static readonly IEnumerable<LocationFactory> MovementTransformations = new List<LocationFactory>
        {
            LocationFactory.Create(LocationFactory.Moves.Right(1), LocationFactory.Moves.Up(2)),
            LocationFactory.Create(LocationFactory.Moves.Right(2), LocationFactory.Moves.Up(1)),
            LocationFactory.Create(LocationFactory.Moves.Right(2), LocationFactory.Moves.Down(1)),
            LocationFactory.Create(LocationFactory.Moves.Right(1), LocationFactory.Moves.Down(2)),
            LocationFactory.Create(LocationFactory.Moves.Left(1), LocationFactory.Moves.Down(2)),
            LocationFactory.Create(LocationFactory.Moves.Left(2), LocationFactory.Moves.Down(1)),
            LocationFactory.Create(LocationFactory.Moves.Left(2), LocationFactory.Moves.Up (1)),
            LocationFactory.Create(LocationFactory.Moves.Left(1), LocationFactory.Moves.Up(2))
        };
    }

    public static class Pawns
    {
        public static readonly RuleSet EnPassantRules = new RuleSet("pawn.enpassant")
                .Add("is-on-correct-rank-to-take",
                    (board, move) => move.From.Rank == EnpassantFromRankFor(board[move.From].Piece.Colour))
                .Add("enemy-pawn-exists-for-enpassant", DestinationIsEnemyPawn)
                .Add("enemy-pawn-has-only-one-move-in-history", PawnBeingTakeWithEnpassantHasCorrectMoveHistory)
//                .Add("friendly-pawn-destination-is-empty", (board, move) => board.IsEmptyAt(move.To))
            ;

        public static bool DestinationIsEnemyPawn(CSharpChess.Board board, Move move)
        {
            var newFile = move.To.File;
            var takeLocation = new BoardLocation(newFile, move.From.Rank);
            var enemyColour = Chess.ColourOfEnemy(board[move.From].Piece.Colour);

            return board[takeLocation].Piece.Is(enemyColour, PieceNames.Pawn);
        }
        public static bool PawnBeingTakeWithEnpassantHasCorrectMoveHistory(CSharpChess.Board board, Move move)
        {
            var newFile = move.To.File;
            var takeLocation = new BoardLocation(newFile, move.From.Rank);
            return board[takeLocation].MoveHistory.Count() == 1;
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
}