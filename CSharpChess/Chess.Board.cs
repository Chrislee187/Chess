using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
// ReSharper disable MemberCanBePrivate.Global

namespace CSharpChess
{
    public static partial class Chess
    {
        public static partial class Board
        {
            public enum ChessFile { A = 1, B, C, D, E, F, G, H };
            public static IEnumerable<ChessFile> Files => new List<ChessFile> {ChessFile.A, ChessFile.B, ChessFile.C, ChessFile.D, ChessFile.E, ChessFile.F, ChessFile.G, ChessFile.H};
            public static IEnumerable<int> Ranks => new [] {1,2,3,4,5,6,7,8};

            public enum DirectionModifiers 
            {
                LeftDirectionModifier = -1,
                RightDirectionModifier = 1,
                UpBoardDirectionModifer = 1,
                DownBoardDirectionModifer = -1,
                NoDirectionModifier = 0
            }

            public static int ForwardDirectionModifierFor(ChessPiece piece)
            {
                return (int) (piece.Colour == Colours.White
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

            public class LocationMover
            {
                public static LocationMover Create(int x, int y) => new LocationMover(x, y);

                public static IEnumerable<BoardLocation> ApplyWhile(BoardLocation from,
                    LocationMover mover, Func<BoardLocation, bool> @while)
                {
                    var result = new List<BoardLocation>();

                    var to = mover.ApplyTo(from);

                    while (to != null && @while(to))
                    {
                        result.Add(to);
                        to = mover.ApplyTo(to);
                    }
                    return result;
                }

                public static IEnumerable<BoardLocation> ApplyToMany(BoardLocation loc, IEnumerable<LocationMover> movers)
                    => movers.Select(m => m.ApplyTo(loc)).Where(l => l != null);

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

                internal static class Moves
                {
                    internal delegate int Transform(int i);
                    internal static Transform Left => (i) => -1 * i;
                    internal static Transform Right => (i) => +1 * i;
                    internal static Transform Down => (i) => -1 * i;
                    internal static Transform Up => (i) => +1 * i;
                    internal static Func<int> None => () => 0;
                }

                private LocationMover(int transformX, int transformY)
                {
                    _transformX = transformX;
                    _transformY = transformY;
                }
            }


        }
    }
}