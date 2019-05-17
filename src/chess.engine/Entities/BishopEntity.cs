using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Game;
using chess.engine.Movement.ChessPieces.Bishop;

namespace chess.engine.Entities
{
    public class BishopEntity : ChessPieceEntity
    {
        public BishopEntity(Colours player) : base(player, ChessPieceName.Bishop)
        {
            Piece = ChessPieceName.Bishop;
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