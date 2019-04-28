using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.King;

namespace chess.engine.Entities
{
    public class KingEntity : ChessPieceEntity
    {
        public KingEntity(Colours owner) : base(ChessPieceName.King, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new KingNormalPathGenerator(),
                // TODO: KingCastleQueenSidePathGenerator()
                // TODO: KingCastleKingSidePathGenerator()
            };

    }
}