using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Pawn;

namespace chess.engine.Entities
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

    }
}