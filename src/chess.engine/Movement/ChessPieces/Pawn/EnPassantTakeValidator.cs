using board.engine;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement.ChessPieces.Pawn
{
    public class EnPassantTakeValidator : IMoveValidator<EnPassantTakeValidator.IBoardStateWrapper> 
    {
        public static IBoardStateWrapper Wrap(IBoardState<ChessPieceEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var normalTakeOk = new DestinationContainsEnemyMoveValidator<ChessPieceEntity>()
                .ValidateMove(move, wrapper.GetDestinationContainsEnemyMoveWrapper());
            if (normalTakeOk) return true;

            var entity = wrapper.GetFromEntity(move);
            if (entity == null) return false;
            var piece = entity.Item;

            var passedEntity = wrapper.GetPassedEntity(move, piece.Player);
            if (passedEntity == null) return false;
            
            if (passedEntity.Item.Is(piece.Player)) return false;
            if (!passedEntity.Item.Is(ChessPieceName.Pawn)) return false;

            return CheckPawnUsedDoubleMove(move.To);
        }

        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }


        public interface IBoardStateWrapper
        {
            DestinationContainsEnemyMoveValidator<ChessPieceEntity>.IBoardStateWrapper
                GetDestinationContainsEnemyMoveWrapper();

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