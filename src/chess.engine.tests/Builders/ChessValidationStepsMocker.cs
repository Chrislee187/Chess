using System.Collections.Generic;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement.King;
using Moq;

namespace chess.engine.tests.Builders
{
    public class ChessValidationStepsMocker
    {
        private readonly Mock<IChessValidationSteps> _castleValidationStepsMock = new Mock<IChessValidationSteps>();

        public IChessValidationSteps Build()
        {
            return _castleValidationStepsMock.Object;
        }

        public void SetupKingCastleEligibility(bool eligible)
        {
            ChessPieceEntity entity = new KingEntity(Colours.White);
            _castleValidationStepsMock.Setup(m =>
                    m.IsKingAllowedToCastle(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        out entity))
                .Returns(eligible);
        }

        public void SetupCastleRookEligibility(bool eligible)
        {
            _castleValidationStepsMock.Setup(m =>
                    m.IsRookAllowedToCastle(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        It.IsAny<Colours>()))
                .Returns(eligible);
        }

        public void SetupPathIsClear(bool clear)
        {
            IEnumerable<BoardLocation> pathBetween;
            _castleValidationStepsMock.Setup(m =>
                    m.IsPathBetweenClear(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        It.IsAny<Colours>(),
                        out pathBetween))
                .Returns(clear);
        }

        public void SetupPathIsSafe(bool safe)
        {
            _castleValidationStepsMock.Setup(m =>
                    m.IsPathClearFromAttacks(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        It.IsAny<IEnumerable<BoardLocation>>()))
                .Returns(safe);
        }

        public void SetupLocationEmpty(bool empty)
        {
            _castleValidationStepsMock.Setup(m =>
                    m.IsLocationEmpty(
                        It.IsAny<BoardLocation>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>()))
                .Returns(empty);
        }

        public void SetupEnpassantFriendlyPawnValid(bool valid, Colours friend)
        {
            ChessPieceEntity pawn = new PawnEntity(friend);
            _castleValidationStepsMock.Setup(m =>
                    m.IsFriendlyPawnValidForEnpassant(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        out pawn))
                .Returns(valid);
        }
        public void SetupEnpassantEnemyPawnValid(bool valid)
        {
            _castleValidationStepsMock.Setup(m =>
                    m.IsEnemyPawnValidForEnpassant(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(), 
                        It.IsAny<Colours>()))
                .Returns(valid);
        }
    }
}