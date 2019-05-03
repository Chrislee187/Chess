using System.Linq;
using chess.engine.Board;

namespace chess.engine.Chess
{
    public class ChessRefreshAllPaths : IRefreshAllPaths
    {
        public void RefreshAllPaths(BoardState boardState, bool removeMovesThatLeaveKingInCheck = true)
        {
            foreach (var loc in boardState.GetAllItemLocations)
            {
                var piece = boardState.GetItem(loc).Item;
                if (piece.EntityType != ChessPieceName.King)
                {
                    boardState.GeneratePaths(piece, loc, removeMovesThatLeaveKingInCheck);
                }
            }
            
            var kings = boardState.GetItems(ChessPieceName.King).ToList();

            boardState.GeneratePaths(kings[0].Item, kings[0].Location, removeMovesThatLeaveKingInCheck);
            boardState.GeneratePaths(kings[1].Item, kings[1].Location, removeMovesThatLeaveKingInCheck);
        }
    }

}