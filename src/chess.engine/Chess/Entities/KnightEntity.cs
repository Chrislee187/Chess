using System.Collections.Generic;
using chess.engine.Chess.Movement.ChessPieces.Knight;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Entities
{
    public class KnightEntity : ChessPieceEntity
    {
        public KnightEntity(Colours owner) : base(ChessPieceName.Knight.ToString(), owner)
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
            return new KnightEntity(Owner);
        }

    }
}