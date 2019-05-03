using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Knight;

namespace chess.engine.Entities
{
    public class KnightEntity : ChessPieceEntity
    {
        public KnightEntity(Colours owner) : base(ChessPieceName.Knight, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new KnightPathGenerator()
            };

        public override object Clone()
        {
            return new KnightEntity(Player);
        }

    }
}