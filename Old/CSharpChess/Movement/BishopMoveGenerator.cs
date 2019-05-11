namespace CSharpChess.Movement
{
    public class BishopMoveGenerator : StraightLineMoveGenerator
    {
        public BishopMoveGenerator() : base(Bishops.MovementTransformations)
        { }
    }
}