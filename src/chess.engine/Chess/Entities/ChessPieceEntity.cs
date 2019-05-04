using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Entities
{
    // TODO: Abstract to a generic, PathGens is the the common thing, piece and owner are chess specific

    public abstract class ChessPieceEntity : IBoardEntity<ChessPieceName, Colours>
    {
        protected ChessPieceEntity(ChessPieceName piece, Colours owner)
        {
            EntityType = piece;
            Owner = owner;
        }
        public ChessPieceName EntityType { get; }

        public Colours Owner { get; }
        public abstract IEnumerable<IPathGenerator> PathGenerators { get; }

        public abstract object Clone();
    }
}