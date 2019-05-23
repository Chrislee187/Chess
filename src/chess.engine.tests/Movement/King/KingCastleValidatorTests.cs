using System.Collections.Generic;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement.King;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests.Movement.King
{
    [TestFixture]
    public class KingCastleValidatorTests
    {
        private KingCastleValidator _validator;
        private Mock<ICastleValidationSteps> _castleValidationStepsMock;

        [SetUp]
        public void Setup()
        {
            _castleValidationStepsMock = new Mock<ICastleValidationSteps>();
            _validator = new KingCastleValidator(_castleValidationStepsMock.Object);
        }
        [Test]
        public void ValidateMove_fails_when_king_is_not_allowed_to_castle()
        {
            SetupKingEligibility(false);
            SetupRookEligibility(false);
            SetupPathIsClear(false);
            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_fails_when_rook_is_not_allowed_to_castle()
        {
            SetupKingEligibility(true);
            SetupRookEligibility(false);
            SetupPathIsClear(false);
            SetupPathIsSafe(false);

            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_fails_when_path_between_is_not_clear()
        {
            SetupKingEligibility(true);
            SetupRookEligibility(true);
            SetupPathIsClear(false);
            SetupPathIsSafe(false);

            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_fails_when_path_between_is_not_safe()
        {
            SetupKingEligibility(true);
            SetupRookEligibility(true);
            SetupPathIsClear(true);
            SetupPathIsSafe(false);

            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_succeeeds_when_all_steps_pass()
        {
            SetupKingEligibility(true);
            SetupRookEligibility(true);
            SetupPathIsClear(true);
            SetupPathIsSafe(true);

            Assert.True(_validator.ValidateMove(null, null));
        }

        private void SetupKingEligibility(bool eligible)
        {
            ChessPieceEntity entity = new KingEntity(Colours.White);
            _castleValidationStepsMock.Setup(m =>
                    m.IsKingAllowedToCastle(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        out entity))
                .Returns(eligible);
        }

        private void SetupRookEligibility(bool eligible)
        {
            _castleValidationStepsMock.Setup(m =>
                    m.IsRookAllowedToCastle(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        It.IsAny<Colours>()))
                .Returns(eligible);
        }
        private void SetupPathIsClear(bool clear)
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

        private void SetupPathIsSafe(bool safe)
        {
            _castleValidationStepsMock.Setup(m =>
                    m.IsPathClearFromAttacks(
                        It.IsAny<BoardMove>(),
                        It.IsAny<IReadOnlyBoardState<ChessPieceEntity>>(),
                        It.IsAny<IEnumerable<BoardLocation>>()))
                .Returns(safe);
        }
    }
}