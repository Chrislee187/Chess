using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement.Validators
{
    public class UpdatePieceValidator<TEntity> : IMoveValidator<TEntity> where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            var piece = boardState.GetItem(move.From).Item;
            var destinationIsEndRank = move.To.Y == ChessGame.EndRankFor((Colours)piece.Owner);
            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState);
            
            return destinationIsEndRank && destinationIsValid;
        }
    }
}