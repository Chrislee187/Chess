namespace CSharpChess.ValidMoves
{
    public class BishopMoveGenerator : StraightLineMoveGenerator
    {
        public BishopMoveGenerator() : base(Chess.Rules.Bishops.DirectionTransformations)
        { }
    }
}