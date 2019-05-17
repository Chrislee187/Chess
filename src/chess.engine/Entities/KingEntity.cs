using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Game;
using chess.engine.Movement.ChessPieces.King;

namespace chess.engine.Entities
{
    public class KingEntity : ChessPieceEntity
    {
        public KingEntity(Colours player) : base(player, ChessPieceName.King)
        {
            Piece = ChessPieceName.King;
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