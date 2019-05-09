using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.Knight;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
{
    public class KnightEntity : ChessPieceEntity
    {
        public KnightEntity(Colours player) : base(player, ChessPieceName.Knight)
        {

            Piece = ChessPieceName.Knight;
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new KnightPathGenerator()
            };

        public override object Clone()
        {
            return new KnightEntity(Player);
        }

    }
}