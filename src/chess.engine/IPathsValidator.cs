using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IPathsValidator<TEntity>
    {
        Paths RemoveInvalidMoves(Paths possiblePaths, IBoardState<TEntity> boardState, bool removeMovesThatLeaveKingInCheck);
        Paths GeneratePossiblePaths(TEntity entity, BoardLocation boardLocation);
        bool DoesMoveLeaveMovingPlayersKingInCheck(BoardMove move, IBoardState<TEntity> boardState);
    }
}