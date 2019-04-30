using System.Linq;
using chess.engine.Board;

namespace chess.engine.Chess
{
    public class ChessRefreshAllPaths : IRefreshAllPaths
    {
        public void RefreshAllPaths(BoardState boardState)
        {
            foreach (var kvp in boardState.Entities)
            {
                var piece = kvp.Value;
                boardState.SetPaths(kvp.Key, null);
                if (piece.EntityType != ChessPieceName.King)
                {
                    boardState.GetOrCreatePaths(kvp.Value, kvp.Key);
                }
            }

            var kings = boardState.Entities.Where(kvp => kvp.Value.EntityType == ChessPieceName.King).ToList();

            boardState.GetOrCreatePaths(kings[0].Value, kings[0].Key);
            boardState.GetOrCreatePaths(kings[1].Value, kings[1].Key);
        }
    }

}