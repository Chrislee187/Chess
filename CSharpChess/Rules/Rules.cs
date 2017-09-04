using System.Collections.Generic;
using System.Linq;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.Rules
{
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

        public static ChessMove CreateRookMoveForCastling(ChessMove move)
        {
            BoardLocation rook;
            BoardLocation rookTo;
            if (move.To.File == CSharpChess.Chess.ChessFile.C)
            {
                rook = BoardLocation.At(Chess.ChessFile.A, move.From.Rank);
                rookTo = BoardLocation.At((Chess.ChessFile)CSharpChess.Chess.ChessFile.D, move.From.Rank);
            }
            else
            {
                rook = BoardLocation.At((Chess.ChessFile)CSharpChess.Chess.ChessFile.H, move.From.Rank);
                rookTo = BoardLocation.At((Chess.ChessFile)CSharpChess.Chess.ChessFile.F, move.From.Rank);
            }

            return new ChessMove(rook, rookTo, MoveType.Castle);
        }

        public static IEnumerable<BoardLocation> SquaresBetweenCastlingPieces(BoardLocation toLoc)
        {
            if (toLoc.File == CSharpChess.Chess.ChessFile.C)
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.B, toLoc.Rank),
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.C, toLoc.Rank),
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.D, toLoc.Rank),
                    };
            }
            else
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.F, toLoc.Rank),
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.G, toLoc.Rank),
                    };
            }

            //                    return Enumerable.Range(fromFile, toFile - fromFile + 1).Select(v => BoardLocation.At(v, fromLoc.Rank));
        }

        public static IEnumerable<BoardLocation> SquaresKingsPassesThroughWhenCastling(BoardLocation toLoc)
        {
            if (toLoc.File == CSharpChess.Chess.ChessFile.C)
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.D, toLoc.Rank),
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.C, toLoc.Rank)
                    };
            }
            else
            {
                return new List<BoardLocation>
                    {
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.F, toLoc.Rank),
                        BoardLocation.At((Chess.ChessFile) CSharpChess.Chess.ChessFile.G, toLoc.Rank),
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
                .Add("friendly-pawn-destination-is-empty", (board, move) => board.IsEmptyAt(move.To))
            ;

        public static bool DestinationIsEnemyPawn(ChessBoard board, ChessMove move)
        {
            var newFile = move.To.File;
            var takeLocation = new BoardLocation(newFile, move.From.Rank);
            var piece = board[takeLocation].Piece;
            var enemyColour = CSharpChess.Chess.ColourOfEnemy(board[move.From].Piece.Colour);

            return board[takeLocation].Piece.Is(enemyColour, CSharpChess.Chess.PieceNames.Pawn);
        }
        public static bool PawnBeingTakeWithEnpassantHasCorrectMoveHistory(ChessBoard board, ChessMove move)
        {
            var newFile = move.To.File;
            var takeLocation = new BoardLocation(newFile, move.From.Rank);
            var piece = board[takeLocation].Piece;
            return board[takeLocation].MoveHistory.Count() == 1;
        }
        public static int EnpassantFromRankFor(CSharpChess.Chess.Colours colour)
        {
            const int whitePawnsEnPassantFromRank = 5;
            const int blackPawnsEnPassantFromRank = 4;

            return colour == CSharpChess.Chess.Colours.White
                ? whitePawnsEnPassantFromRank
                : colour == CSharpChess.Chess.Colours.Black
                    ? blackPawnsEnPassantFromRank : 0;

        }
        public static int StartingPawnRankFor(CSharpChess.Chess.Colours chessPieceColour)
        {
            if (chessPieceColour == CSharpChess.Chess.Colours.White)
                return 2;

            if (chessPieceColour == CSharpChess.Chess.Colours.Black)
                return 7;

            return 0;
        }
        public static int PromotionRankFor(CSharpChess.Chess.Colours chessPieceColour)
        {
            return chessPieceColour == CSharpChess.Chess.Colours.Black
                ? 1
                : chessPieceColour == CSharpChess.Chess.Colours.White
                    ? 8
                    : 0;
        }
    }
}