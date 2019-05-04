using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.Rook;

namespace chess.engine.Chess.Entities
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

        public override object Clone()
        {
            return new RookEntity(Player);
        }
    }
}