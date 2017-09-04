using CSharpChess.Rules;

namespace CSharpChess.MoveGeneration
{
    public class BishopMoveGenerator : StraightLineMoveGenerator
    {
        public BishopMoveGenerator() : base(Bishops.MovementTransformations)
        { }
    }
}