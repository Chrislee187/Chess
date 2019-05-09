using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
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