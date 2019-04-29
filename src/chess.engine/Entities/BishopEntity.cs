using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.King;

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

    }
}