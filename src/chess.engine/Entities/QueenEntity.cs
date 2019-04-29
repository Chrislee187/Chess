using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Queen;
using chess.engine.Pieces.Rook;

namespace chess.engine.Entities
{
    public class QueenEntity : ChessPieceEntity
    {
        public QueenEntity(Colours owner) : base(ChessPieceName.Queen, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new QueenPathGenerator()
            };

    }
}