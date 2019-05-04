using chess.engine.Board;
using chess.engine.Chess;

namespace chess.engine.Movement.Validators
{
    public class UpdatePieceValidator : IMoveValidator
    {
        public bool ValidateMove(BoardMove move, IBoardState boardState)
        {
            var piece = boardState.GetItem(move.From).Item;
            var destinationIsEndRank = move.To.Rank == ChessGame.EndRankFor(piece.Owner);
            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator().ValidateMove(move, boardState);
            
            return destinationIsEndRank && destinationIsValid;
        }
    }
}