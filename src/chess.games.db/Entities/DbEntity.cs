using System;

namespace chess.games.db.Entities
{
    public abstract class DbEntity
    {
        public Guid Id { get; private set; }
    }
}