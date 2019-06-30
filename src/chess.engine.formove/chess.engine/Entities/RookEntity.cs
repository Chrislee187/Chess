using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Game;
using chess.engine.Movement.Rook;

namespace chess.engine.Entities
{
    public class RookEntity : ChessPieceEntity
    {
        public RookEntity(Colours player) : base(player, ChessPieceName.Rook)
        {
            Piece = ChessPieceName.Rook;
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