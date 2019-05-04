using System.Collections.Generic;
using chess.engine.Chess.Movement.ChessPieces.Queen;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Entities
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
        public override object Clone()
        {
            return new QueenEntity(Owner);
        }

    }
}