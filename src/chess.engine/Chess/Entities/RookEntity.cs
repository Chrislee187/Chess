using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.Rook;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
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