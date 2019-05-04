using System.Collections.Generic;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Entities
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
            return new KingEntity(Owner);
        }

    }
}