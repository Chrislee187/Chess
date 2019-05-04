using System.Collections.Generic;
using chess.engine.Chess.Movement.ChessPieces.Pawn;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Entities
{
    public class PawnEntity : ChessPieceEntity
    {
        public PawnEntity(Colours owner) : base(ChessPieceName.Pawn, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new PawnNormalAndStartingPathGenerator(),
                new PawnRightTakePathGenerator(),
                new PawnLeftTakePathGenerator()
            };
        public override object Clone()
        {
            return new PawnEntity(Owner);
        }

    }
}