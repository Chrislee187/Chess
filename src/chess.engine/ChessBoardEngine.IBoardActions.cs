using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine
{
    public interface IBoardActions
    {
        ChessPieceEntity GetEntity(BoardLocation loc);
        void ClearSquare(BoardLocation loc);
        void PlaceEntity(BoardLocation loc, ChessPieceEntity entity);

    }
}