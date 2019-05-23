using System.Collections.Generic;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement.King
{
    public class KingCastleValidator : IMoveValidator<ChessPieceEntity>
    {
        private readonly ICastleValidationSteps _validationSteps;

        public KingCastleValidator(ICastleValidationSteps validationSteps)
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

    public interface ICastleValidationSteps
    {
        bool IsPathClearFromAttacks(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState, IEnumerable<BoardLocation> pathBetween);

        bool IsPathBetweenClear(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState,
            Colours kingColour, out IEnumerable<BoardLocation> pathBetween);

        bool IsRookAllowedToCastle(BoardMove move,
            IReadOnlyBoardState<ChessPieceEntity> roBoardState, Colours player);

        bool IsKingAllowedToCastle(BoardMove move,
            IReadOnlyBoardState<ChessPieceEntity> roBoardState,
            out ChessPieceEntity king);
    }
}