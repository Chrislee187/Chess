namespace CSharpChess.ValidMoves
{
    public class RookValidMoveGenerator : StraightLineValidMoveGenerator
    {
        public RookValidMoveGenerator() : base(Chess.Rules.Rooks.DirectionTransformations)
        { }
    }
}