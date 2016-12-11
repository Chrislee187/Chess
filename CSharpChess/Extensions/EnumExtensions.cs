using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> All<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool IsTake(this MoveType m)
        {
            var takeTypes = new[] {MoveType.Take, MoveType.TakeEnPassant};
            return takeTypes.Any(mt => mt == m);
        }
        public static bool IsMove(this MoveType m)
        {
            var takeTypes = new[] { MoveType.Move, MoveType.Castle, MoveType.Promotion};
            return takeTypes.Any(mt => mt == m);
        }
        public static bool IsCover(this MoveType m)
        {
            var takeTypes = new[] { MoveType.Cover };
            return takeTypes.Any(mt => mt == m);
        }
    }
}