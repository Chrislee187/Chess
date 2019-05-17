using System;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationContainsEnemyMoveValidator<TEntity>
        : IMoveValidator<TEntity, DestinationContainsEnemyMoveValidator<TEntity>.IBoardStateWrapper>
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var sourcePiece = wrapper.GetFromEntity(move);
            if (sourcePiece == null) return false;

            var destinationPiece = wrapper.GetToEntity(move);
            if (destinationPiece == null) return false;

            return sourcePiece.Item.Owner != destinationPiece.Item.Owner;
        }

        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            throw new NotImplementedException();
//            IBoardStateWrapper wrapper = new BoardStateWrapper(boardState);
//
//            if (boardState.IsEmpty(move.From))
//                return false; //throw new SystemException($"Piece missing at {move.From}");
//            var sourcePiece = wrapper.GetFromEntity(move);
//
//            if (!boardState.IsEmpty(move.To))
//            {
//                var destinationPiece = wrapper.GetToEntity(move);
//                return sourcePiece.Item.Owner != destinationPiece.Item.Owner;
//            }
//
//            return false;
        }

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetFromEntity(BoardMove move);
            LocatedItem<TEntity> GetToEntity(BoardMove move);
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState) { }
        }
    }
}