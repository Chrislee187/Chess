using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Entities
{
    // TODO: Abstract to a generic, PathGens is the the common thing, piece and player are chess specific

    public abstract class ChessPieceEntity : IBoardEntity
    {
        public int EntityType { get; }
        public string EntityName { get; }
        public int Owner { get; }


        public Colours Player { get; }
        public ChessPieceName Piece { get; protected set; }

        protected ChessPieceEntity(Colours player, ChessPieceName piece)
        {
            EntityType = (int) piece;
            EntityName = piece.ToString();
            Owner = (int) player;

            Player = player;
            Piece = piece;
        }

        public abstract IEnumerable<IPathGenerator> PathGenerators { get; }

        public abstract object Clone();

    }
}