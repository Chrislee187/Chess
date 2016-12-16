using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.System.Extensions
{
    public static class MoveTypeExtensions
    { 
        public static bool IsTake(this MoveType m) 
            => new[] {MoveType.Take, MoveType.TakeEnPassant}.Any(mt => mt == m);

        public static bool IsMove(this MoveType m) 
            => new[] { MoveType.Move, MoveType.Castle, MoveType.Promotion}.Any(mt => mt == m);

        public static bool IsCover(this MoveType m) 
            => new[] { MoveType.Cover }.Any(mt => mt == m);
    }
}