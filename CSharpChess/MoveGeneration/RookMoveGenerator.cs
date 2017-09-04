using CSharpChess.Rules;

namespace CSharpChess.MoveGeneration
{
    public class RookMoveGenerator : StraightLineMoveGenerator
    {
        public RookMoveGenerator() : base(Rooks.MovementTransformations)
        { }
    }
}