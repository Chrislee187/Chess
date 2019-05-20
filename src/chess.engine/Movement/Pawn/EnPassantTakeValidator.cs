using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class EnPassantTakeValidator : IMoveValidator<EnPassantTakeValidator.IBoardStateWrapper> 
    {
        public static IBoardStateWrapper Wrap(IBoardState<ChessPieceEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var destEmpty = new DestinationIsEmptyValidator<ChessPieceEntity>()
                .ValidateMove(move, wrapper.GetDestinationIsEmptyWrapper());
            if (!destEmpty) return false;

            var entity = wrapper.GetFromEntity(move);
            if (entity == null) return false;
            var piece = entity.Item;

            var passedEntity = wrapper.GetPassedEntity(move, piece.Player);
            if (passedEntity == null) return false;
            
            if (!passedEntity.Item.Is(piece.Player.Enemy(), ChessPieceName.Pawn)) return false;

            return CheckPawnUsedDoubleMove(passedEntity);
        }

        private bool CheckPawnUsedDoubleMove(LocatedItem<ChessPieceEntity> moveTo)
        {
            // Doesn't check it was the LAST move
            return moveTo.Item.LocationHistory.Count == 1;
        }


        public interface IBoardStateWrapper
        {
            DestinationIsEmptyValidator<ChessPieceEntity>.IBoardStateWrapper
                GetDestinationIsEmptyWrapper();

            LocatedItem<ChessPieceEntity> GetFromEntity(BoardMove move);

            LocatedItem<ChessPieceEntity> GetPassedEntity(BoardMove move, Colours colour);
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<ChessPieceEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<ChessPieceEntity> boardState) : base(boardState)
            {
            }

            public LocatedItem<ChessPieceEntity> GetPassedEntity(BoardMove move, Colours colour)
            {
                var passingPieceLocation = move.To.MoveBack(colour);

                return passingPieceLocation == null 
                    ? null 
                    : BoardState.GetItem(passingPieceLocation);
            }
        }
    }
}