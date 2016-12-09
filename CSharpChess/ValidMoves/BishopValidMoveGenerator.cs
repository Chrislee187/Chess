using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class BishopValidMoveGenerator : StraightLineValidMoveGenerator
    {
        public BishopValidMoveGenerator() : base(Chess.Rules.Bishops.DirectionTransformations, Chess.PieceNames.Bishop)
        { }
    }
}