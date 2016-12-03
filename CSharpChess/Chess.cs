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

            public static int Direction(ChessPiece piece)
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

        public static Colours EnemyColour(Colours colour)
        {
            return colour == Colours.Black
                ? Colours.White
                : colour == Colours.White
                    ? Colours.Black
                    : colour;
        }

        public static bool CanTakeAt(ChessBoard board, BoardLocation takeLocation, Colours attackerColour)
            => board.IsNotEmptyAt(takeLocation)
            && board[takeLocation].Piece.Colour == EnemyColour(attackerColour);
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
                private static Func<int, int> left => (i) => -1 * i;
                private static Func<int, int> right => (i) => +1 * i;
                private static Func<int, int> down => (i) => -1 * i;
                private static Func<int, int> up => (i) => +1 * i;
                public static IEnumerable<Tuple<int,int>> MoveMatrix = new List<Tuple<int, int>>
                {
                    Tuple.Create(right(1), up(2)),
                    Tuple.Create(right(2), up(1)),
                    Tuple.Create(right(2), down(1)),
                    Tuple.Create(right(1), down(2)),
                    Tuple.Create(left(1), down(2)),
                    Tuple.Create(left(2), down(1)),
                    Tuple.Create(left(2), up (1)),
                    Tuple.Create(left(1), up(2))
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
        }
    }
}