namespace CSharpChess.Movement
{
    public class RookMoveGenerator : StraightLineMoveGenerator
    {
        public RookMoveGenerator() : base(Rooks.MovementTransformations)
        { }
    }
}