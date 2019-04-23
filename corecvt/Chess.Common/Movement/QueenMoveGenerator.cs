namespace Chess.Common.Movement
{
    public class QueenMoveGenerator : StraightLineMoveGenerator
    {
        public QueenMoveGenerator() : base(Queen.MovementTransformations)
        { }
    }
}