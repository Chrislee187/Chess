using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public static class Chess
    {
        public static IEnumerable<int> Ranks
        {
            get
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    yield return rank;
                }
            }
        }

        public static IEnumerable<ChessFile> Files => EnumExtensions.All<ChessFile>();

        public static class Board
        {

            public const int LeftDirectionModifier = -1;
            public const int RightDirectionModifier = 1;


            public static bool IsValidLocation(int file, int rank)
            {
                return !Validations.InvalidFile(file)
                    && !Validations.InvalidRank(rank);
            }
        }

        public enum ChessFile { A = 1, B, C, D, E, F, G, H };

        public static class Pieces
        {
            public static readonly ChessPiece Blank = ChessPiece.NullPiece;

            public static class White
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Colours.White, PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Colours.White, PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Colours.White, PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Colours.White, PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Colours.White, PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Colours.White, PieceNames.Queen);
            }
            public static class Black
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Colours.Black, PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Colours.Black, PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Colours.Black, PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Colours.Black, PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Colours.Black, PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Colours.Black, PieceNames.Queen);
            }

            public static int VerticalDirectionModifierFor(ChessPiece piece)
            {
                return piece.Colour == Colours.White
                    ? +1
                    : piece.Colour == Colours.Black
                        ? -1 : 0;

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
        }

        public static Colours ColourOfEnemy(Colours colour)
        {
            return colour == Colours.Black
                ? Colours.White
                : colour == Colours.White
                    ? Colours.Black
                    : colour;
        }

        public static bool CanTakeAt(ChessBoard board, BoardLocation takeLocation, Colours attackerColour)
            => board.IsNotEmptyAt(takeLocation)
            && board[takeLocation].Piece.Colour == ColourOfEnemy(attackerColour);
        public enum Colours
        {
            White, Black,
            None
        }

        public enum PieceNames
        {
            Pawn, Rook, Bishop, Knight, King, Queen, Blank = -9999
        }

        public static class Validations
        {
            public static bool InvalidRank(int rank) => !Ranks.Contains(rank);

            public static bool InvalidFile(ChessFile file) => InvalidFile((int)file);
            public static bool InvalidFile(int file) => Files.All(f => (int) f != file);

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
                ThrowInvalidFile((int) file);
            }
        }

        public static int StartingPawnRankFor(Colours chessPieceColour)
        {
            if (chessPieceColour == Colours.White)
                return 2;
            else if (chessPieceColour == Colours.Black)
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

        public static class Rules
        {
            private static class Movement
            {
                internal static Func<int, int> Left => (i) => -1 * i;
                internal static Func<int, int> Right => (i) => +1 * i;
                internal static Func<int, int> Down => (i) => -1 * i;
                internal static Func<int, int> Up => (i) => +1 * i;
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
            }

            public static class Knights
            {
                private static readonly IEnumerable<Tuple<int,int>> MoveMatrix = new List<Tuple<int, int>>
                {
                    Tuple.Create(Movement.Right(1), Movement.Up(2)),
                    Tuple.Create(Movement.Right(2), Movement.Up(1)),
                    Tuple.Create(Movement.Right(2), Movement.Down(1)),
                    Tuple.Create(Movement.Right(1), Movement.Down(2)),
                    Tuple.Create(Movement.Left(1), Movement.Down(2)),
                    Tuple.Create(Movement.Left(2), Movement.Down(1)),
                    Tuple.Create(Movement.Left(2), Movement.Up (1)),
                    Tuple.Create(Movement.Left(1), Movement.Up(2))
                };

                public static IEnumerable<BoardLocation> MovesFrom(BoardLocation from)
                {
                    return MoveMatrix.Where(t =>
                    {
                        var file = (int) from.File + t.Item1;
                        var rank = from.Rank + t.Item2;

                        return Board.IsValidLocation(file, rank);
                    }).Select(t =>
                    {
                        var file = (int) from.File + t.Item1;
                        var rank = from.Rank + t.Item2;
                        return BoardLocation.At(file, rank);
                    });
                }
            }

            public static class Bishops
            {
                public static IEnumerable<Tuple<int, int>> DirectionTransformations => new List<Tuple<int, int>>
                {
                    Tuple.Create(Movement.Right(1), Movement.Up(1)),
                    Tuple.Create(Movement.Right(1), Movement.Down(1)),
                    Tuple.Create(Movement.Left(1), Movement.Up(1)),
                    Tuple.Create(Movement.Left(1), Movement.Down(1))
                };
            }
        }
    }
}