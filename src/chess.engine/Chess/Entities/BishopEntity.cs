using System.Collections.Generic;
using chess.engine.Chess.Movement.ChessPieces.Bishop;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Entities
{
    public class BishopEntity : ChessPieceEntity
    {
        public BishopEntity(Colours owner) : base(ChessPieceName.Bishop.ToString(), owner)
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
            return new BishopEntity(Owner);
        }
    }
}