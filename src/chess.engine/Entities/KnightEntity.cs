using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Knight;

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

    }
}