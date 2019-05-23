using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;

namespace chess.engine.Movement.King
{
    public class KingCastleValidator : IMoveValidator<ChessPieceEntity>
    {
        private readonly IChessValidationSteps _validationSteps;

        public KingCastleValidator(IChessValidationSteps validationSteps)
        {
            _validationSteps = validationSteps;
        }

        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState)
        {
            if (!_validationSteps.IsKingAllowedToCastle(move, roBoardState, out var king)) return false;

            if (!_validationSteps.IsRookAllowedToCastle(move, roBoardState, king.Player)) return false;

            if (!_validationSteps.IsPathBetweenClear(move, roBoardState, king.Player, out var pathBetween)) return false;

            return _validationSteps.IsPathClearFromAttacks(move, roBoardState, pathBetween);
        }
    }

}