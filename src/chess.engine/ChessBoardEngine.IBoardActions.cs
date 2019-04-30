using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine
{
    public interface ILiveBoardActions
    {
        ChessPieceEntity GetEntity(BoardLocation loc);
        void ClearSquare(BoardLocation loc);
        void PlaceEntity(BoardLocation loc, ChessPieceEntity entity);

    }
}