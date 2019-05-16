using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;

namespace board.engine
{
    public class BoardMoveService<TEntity> : IBoardMoveService<TEntity>
        where TEntity : class, IBoardEntity
    {
        private readonly IBoardActionProvider<TEntity> _boardActionProvider;

        public BoardMoveService(IBoardActionProvider<TEntity> boardActionProvider)
        {
            _boardActionProvider = boardActionProvider;
        }

        public void Move(IBoardState<TEntity> boardState, BoardMove move)
        {
            var action = _boardActionProvider.Create((int)move.MoveType, boardState);

            action.Execute(move);
        }
    }

    public interface IBoardMoveService<TEntity>
        where TEntity : class, IBoardEntity
    {
        void Move(IBoardState<TEntity> boardState, BoardMove move);
    }
}