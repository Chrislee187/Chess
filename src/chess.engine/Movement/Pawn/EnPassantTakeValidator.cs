using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class EnPassantTakeValidator : IMoveValidator<ChessPieceEntity> 
    {
        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState)
        {
            var destEmpty = new DestinationIsEmptyValidator<ChessPieceEntity>()
                .ValidateMove(move, roBoardState);
            if (!destEmpty) return false;

            var entity = roBoardState.GetItem(move.From);
            if (entity == null) return false;
            var piece = entity.Item;

            var passingPieceLocation = move.To.MoveBack(piece.Player);
            var passedEntity = roBoardState.GetItem(passingPieceLocation);

            if (passedEntity == null) return false;
            
            if (!passedEntity.Item.Is(piece.Player.Enemy(), ChessPieceName.Pawn)) return false;

            return CheckPawnUsedDoubleMove(passedEntity);
        }

        private bool CheckPawnUsedDoubleMove(LocatedItem<ChessPieceEntity> moveTo)
        {
            // Doesn't check it was the LAST move
            return moveTo.Item.LocationHistory.Count == 1;
        }
    }
}