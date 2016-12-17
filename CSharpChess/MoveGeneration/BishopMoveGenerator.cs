namespace CSharpChess.MoveGeneration
{
    public class BishopMoveGenerator : StraightLineMoveGenerator
    {
        public BishopMoveGenerator() : base(Chess.Rules.Bishops.MovementTransformations)
        { }
    }
}