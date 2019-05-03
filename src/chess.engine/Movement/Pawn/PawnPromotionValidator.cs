using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement.SimpleValidators;

namespace chess.engine.Movement.Pawn
{
    public class PawnPromotionValidator : IMoveValidator
    {

        public bool ValidateMove(ChessMove move, IBoardState boardState)
        {
            var piece = boardState.GetItem(move.From).Item;
            var destinationIsEndRank = move.To.Rank == ChessGame.EndRankFor(piece.Player);
            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator().ValidateMove(move, boardState);
            
            return destinationIsEndRank && destinationIsValid;
        }
    }
}