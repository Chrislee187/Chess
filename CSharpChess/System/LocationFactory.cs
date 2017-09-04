using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.System
{
    public class LocationFactory
    {
        public static LocationFactory Create(int x, int y) => new LocationFactory(x, y);

        public static IEnumerable<BoardLocation> ApplyWhile(BoardLocation from,
            LocationFactory factory, Func<BoardLocation, bool> @while)
        {
            var result = new List<BoardLocation>();

            var to = factory.ApplyTo(@from);

            while (to != null && @while(to))
            {
                result.Add(to);
                to = factory.ApplyTo(to);
            }
            return result;
        }

        public static IEnumerable<BoardLocation> ApplyToMany(BoardLocation loc, IEnumerable<LocationFactory> movers)
            => movers.Select(m => m.ApplyTo(loc)).Where(l => l != null);

        public BoardLocation ApplyTo(BoardLocation from)
        {
            var file = (int)@from.File + _transformX;
            var rank = @from.Rank + _transformY;
            return !Validations.IsValidLocation(file, rank)
                ? null
                : new BoardLocation((Chess.ChessFile)file, rank);
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

        private LocationFactory(int transformX, int transformY)
        {
            _transformX = transformX;
            _transformY = transformY;
        }
    }
}