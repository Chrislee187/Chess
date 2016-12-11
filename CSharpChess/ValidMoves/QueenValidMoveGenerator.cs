namespace CSharpChess.ValidMoves
{
    public class QueenValidMoveGenerator : StraightLineValidMoveGenerator
    {
        public QueenValidMoveGenerator() : base(Chess.Rules.KingAndQueen.DirectionTransformations)
        { }
    }
}