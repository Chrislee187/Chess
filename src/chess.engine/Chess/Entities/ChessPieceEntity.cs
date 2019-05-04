using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Entities
{
    // TODO: Abstract to a generic, PathGens is the the common thing, piece and owner are chess specific

    public abstract class ChessPieceEntity : IBoardEntity<ChessPieceName>
    {
        protected ChessPieceEntity(ChessPieceName piece, Colours owner)
        {
            EntityType = piece;
            Player = owner;
        }
        public ChessPieceName EntityType { get; }

        public Colours Player { get; }
        public abstract IEnumerable<IPathGenerator> PathGenerators { get; }

        public abstract object Clone();
    }
}