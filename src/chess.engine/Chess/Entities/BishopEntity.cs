using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.Bishop;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
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