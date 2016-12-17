namespace CSharpChess.MoveGeneration
{
    public class RookMoveGenerator : StraightLineMoveGenerator
    {
        public RookMoveGenerator() : base(Chess.Rules.Rooks.MovementTransformations)
        { }
    }
}