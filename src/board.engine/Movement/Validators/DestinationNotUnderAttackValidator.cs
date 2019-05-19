using System.Collections.Generic;
using System.Linq;
using board.engine.Actions;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationNotUnderAttackValidator<TEntity> 
        : IMoveValidator<DestinationNotUnderAttackValidator<TEntity>.IBoardStateWrapper> 
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var piece = wrapper.GetFromEntity(move);
            if (piece == null) return false;

            var owner = piece.Item.Owner;

            var enemyPaths = new Paths();
            var enemyItems = wrapper.GetNonOwnerEntities(owner);
            enemyPaths.AddRange(enemyItems.SelectMany(li => li.Paths));

            var attackMoveTypes = new []{ (int) DefaultActions.MoveOrTake, (int) DefaultActions.TakeOnly, (int) ChessMoveTypes.KingMove };
            return !enemyPaths.ContainsMoveTypeTo(move.To, attackMoveTypes);
        }

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetFromEntity(BoardMove move);
            IEnumerable<LocatedItem<TEntity>> GetNonOwnerEntities(int owner);
        }
        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState) { }

            public IEnumerable<LocatedItem<TEntity>> GetNonOwnerEntities(int owner)
                => BoardState.GetNonOwnerItems(owner);
        }
    }
}