using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Game;
using chess.engine.Movement.ChessPieces.Knight;

namespace chess.engine.Entities
{
    public class KnightEntity : ChessPieceEntity
    {
        public KnightEntity(Colours player) : base(player, ChessPieceName.Knight)
        {

            Piece = ChessPieceName.Knight;
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