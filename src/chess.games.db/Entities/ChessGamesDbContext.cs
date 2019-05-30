using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace chess.games.db.Entities
{
    public class ChessGamesDbContext : DbContext
    {
        private readonly string _dataSourceDSrcChessSrcChessGamesDbChessgamesDb =
            "Data Source=D:\\src\\chess\\src\\chess.games.db\\chessgames.db";
        public DbSet<Event> Events { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }

        public ChessGamesDbContext()
        {
        }



        public ChessGamesDbContext(string sqlLiteDbFile)
        {
            _dataSourceDSrcChessSrcChessGamesDbChessgamesDb = $"Data Source={sqlLiteDbFile }" ;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_dataSourceDSrcChessSrcChessGamesDbChessgamesDb);
        }

        // NOTE: Resharper tries to use the interface returned by Include(), (IIncludableQueryable) which is wrong and
        // stops the Includes working until a ToList(). (Any()/Single() etc. receive null children)
        public IEnumerable<Game> GamesWithIncludes()
        {
            return Games
                .Include(i => i.Black)
                .Include(i => i.White)
                .Include(i => i.Event)
                .Include(i => i.Site);
        }

        public TEntity GetOrCreate<TEntity>(
            Func<TEntity, bool> matcher,
            Func<TEntity> builder
        ) where TEntity : class
            => Set<TEntity>().Any(matcher)
                ? Set<TEntity>().Single(matcher)
                : builder();
    }
}