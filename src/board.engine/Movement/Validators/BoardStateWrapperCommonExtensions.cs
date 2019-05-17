using System.Collections.Generic;
using System.Linq;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public static class BoardStateWrapperCommonExtensions
    {
        public static LocatedItem<TEntity> GetFromItem<TEntity>(this IBoardState<TEntity> boardState,
            BoardMove move)
            where TEntity : class, IBoardEntity
            => boardState.GetItem(move.From);

        public static LocatedItem<TEntity> GetToItem<TEntity>(this IBoardState<TEntity> boardState,
            BoardMove move)
            where TEntity : class, IBoardEntity
            => boardState.GetItem(move.To);

        public static IEnumerable<LocatedItem<TEntity>> GetOwnerItems<TEntity>(this IBoardState<TEntity> boardState,
            int owner)
            where TEntity : class, IBoardEntity
            => boardState.GetItems().Where(i => i.Item.Owner == owner);

        public static IEnumerable<LocatedItem<TEntity>> GetNonOwnerItems<TEntity>(this IBoardState<TEntity> boardState,
            int owner)
            where TEntity : class, IBoardEntity
            => boardState.GetItems().Where(i => i.Item.Owner != owner);
    }
}