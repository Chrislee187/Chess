using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.Bishop;

namespace chess.engine.Chess.Entities
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