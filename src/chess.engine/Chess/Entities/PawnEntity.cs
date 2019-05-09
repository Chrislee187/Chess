using System.Collections.Generic;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.Pawn;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
{
    public class PawnEntity : ChessPieceEntity
    {
        public PawnEntity(Colours player) : base(player, ChessPieceName.Pawn)
        {
            Piece = ChessPieceName.Pawn;
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
            return new PawnEntity(Player);
        }

    }
}