using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Game;
using chess.engine.Movement.Queen;

namespace chess.engine.Entities
{
    public class QueenEntity : ChessPieceEntity
    {
        public QueenEntity(Colours player) : base(player, ChessPieceName.Queen)
        {
            Piece = ChessPieceName.Queen;
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new QueenPathGenerator()
            };
        public override object Clone()
        {
            return new QueenEntity(Player);
        }

    }
}