using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.King;

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
                new KingCastlePathGenerator()
            };

        public override object Clone()
        {
            return new KingEntity(Player);
        }

    }
}