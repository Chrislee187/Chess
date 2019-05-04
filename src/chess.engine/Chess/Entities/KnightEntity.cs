using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.Knight;

namespace chess.engine.Chess.Entities
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