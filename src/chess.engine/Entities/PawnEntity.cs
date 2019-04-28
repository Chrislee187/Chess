using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Pawn;

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
                new PawnRightTakePathGenerator(), new PawnNormalAndStartingPathGenerator(), new PawnLeftTakePathGenerator()
            };

    }
}