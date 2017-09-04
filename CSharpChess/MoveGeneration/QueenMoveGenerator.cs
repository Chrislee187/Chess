using CSharpChess.Rules;

namespace CSharpChess.MoveGeneration
{
    public class QueenMoveGenerator : StraightLineMoveGenerator
    {
        public QueenMoveGenerator() : base(Queen.MovementTransformations)
        { }
    }
}