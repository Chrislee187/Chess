using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Movement.King;

namespace chess.engine.Movement.Pawn
{
    public class EnPassantTakeValidator : IMoveValidator<ChessPieceEntity> 
    {
        private readonly IChessValidationSteps _validationSteps;

        public EnPassantTakeValidator(IChessValidationSteps validationSteps)
        {
            _validationSteps = validationSteps;
        }

        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState)
        {
            if (!_validationSteps.IsLocationEmpty(move.To, roBoardState)) return false;

            if (!_validationSteps.IsFriendlyPawnValidForEnpassant(move, roBoardState, out var pawn)) return false;

            var isEnemyPawnValidForEnpassant = _validationSteps.IsEnemyPawnValidForEnpassant(move, roBoardState, pawn.Player);
            return isEnemyPawnValidForEnpassant;
        }
    }
}