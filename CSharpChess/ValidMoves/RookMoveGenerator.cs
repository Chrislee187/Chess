namespace CSharpChess.ValidMoves
{
    public class RookMoveGenerator : StraightLineMoveGenerator
    {
        public RookMoveGenerator() : base(Chess.Rules.Rooks.DirectionTransformations)
        { }
    }
}