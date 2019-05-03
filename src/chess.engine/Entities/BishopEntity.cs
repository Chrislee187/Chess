using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Bishop;

namespace chess.engine.Entities
{
    public class BishopEntity : ChessPieceEntity
    {
        public BishopEntity(Colours owner) : base(ChessPieceName.Bishop, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new BishopPathGenerator()
            };

        public override object Clone()
        {
            return new BishopEntity(Player);
        }
    }
}