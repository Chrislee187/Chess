using System.Linq;
using chess.engine.Board;

namespace chess.engine.Chess
{
    public class ChessRefreshAllPaths : IRefreshAllPaths
    {
        public void RefreshAllPaths(BoardState boardState)
        {
            foreach (var loc in boardState.LocationsInUse)
            {
                var piece = boardState.GetItem(loc).Item;
                if (piece.EntityType != ChessPieceName.King)
                {
                    boardState.UpdatePaths(piece, loc);
                }
            }
            
            var kings = boardState.Get(ChessPieceName.King).ToList();

            boardState.UpdatePaths(kings[0].Item, kings[0].Location);
            boardState.UpdatePaths(kings[1].Item, kings[1].Location);
        }
    }

}