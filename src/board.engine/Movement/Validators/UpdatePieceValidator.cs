using System;
using System.Net.NetworkInformation;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class UpdatePieceValidator<TEntity> : IMoveValidator<TEntity, UpdatePieceValidator<TEntity>.IBoardStateWrapper> where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            throw new NotImplementedException();
//            var piece = boardState.GetItem(move.From).Item;
//
//            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState);
//            
//            return destinationIsValid;
        }



        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var piece = wrapper.GetFromEntity(move);
            if (piece == null) return false;

            var subWrapper = wrapper.GetDestinationIsEmptyOrContainsEnemyWrapper();
            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator<TEntity>()
                .ValidateMove(move, subWrapper);

            return destinationIsValid;
        }

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetFromEntity(BoardMove move);

            DestinationIsEmptyOrContainsEnemyValidator<TEntity>.IBoardStateWrapper
                GetDestinationIsEmptyOrContainsEnemyWrapper();
        }
        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState) { }
        }
    }
}