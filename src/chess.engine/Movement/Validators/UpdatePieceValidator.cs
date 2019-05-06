using chess.engine.Board;
using chess.engine.Chess;

namespace chess.engine.Movement.Validators
{
    public class UpdatePieceValidator<TEntity> : IMoveValidator<TEntity> where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            var piece = boardState.GetItem(move.From).Item;
            var destinationIsEndRank = move.To.Rank == ChessGame.EndRankFor(piece.Owner);
            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState);
            
            return destinationIsEndRank && destinationIsValid;
        }
    }
}