using System;
using System.Runtime.InteropServices;

namespace chess.games.db.Entities
{
    public abstract class DbEntity
    {
        public Guid Id { get; private set; }
    }
    public class Event : DbEntity
    {
        public string Name { get; set; }
    }
    public class Site : DbEntity
    {
        public string Name { get; set; }
    }
    public class Player : DbEntity
    {
        public string Name { get; set; }
    }

    public class Game
    {
        public Event Event { get; set; }
        public Site Site { get; set; }
        public Player White { get; set; }
        public Player Black { get; set; }

        public DateTime Date { get; set; }

        public string Round { get; set; }
        public GameResult Result { get; set; }

        public string MoveText { get; set; }
    }

    public enum GameResult
    {
        Unknown, WhiteWins, BlackWins, Draw
    }

}
