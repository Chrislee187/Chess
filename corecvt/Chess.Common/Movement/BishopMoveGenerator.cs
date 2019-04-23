namespace Chess.Common.Movement
{
    public class BishopMoveGenerator : StraightLineMoveGenerator
    {
        public BishopMoveGenerator() : base(Bishops.MovementTransformations)
        { }
    }
}