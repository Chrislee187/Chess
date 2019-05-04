using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.Queen;

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
            return new QueenEntity(Player);
        }

    }
}