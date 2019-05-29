using Microsoft.EntityFrameworkCore;

namespace chess.games.db.Entities
{
    public class ChessGamesDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\src\\chess\\src\\chess.games.db\\chessgames.db");
        }
    }
}