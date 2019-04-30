using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Rook;

namespace chess.engine.Entities
{
    public class RookEntity : ChessPieceEntity
    {
        public RookEntity(Colours owner) : base(ChessPieceName.Rook, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new RookPathGenerator()
            };

    }
}