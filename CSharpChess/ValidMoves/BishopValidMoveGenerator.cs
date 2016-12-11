namespace CSharpChess.ValidMoves
{
    public class BishopValidMoveGenerator : StraightLineValidMoveGenerator
    {
        public BishopValidMoveGenerator() : base(Chess.Rules.Bishops.DirectionTransformations)
        { }
    }
}