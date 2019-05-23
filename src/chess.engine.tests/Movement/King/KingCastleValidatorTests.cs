using chess.engine.Movement.King;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.King
{
    [TestFixture]
    public class KingCastleValidatorTests
    {
        private KingCastleValidator _validator;
        private readonly ChessValidationStepsMocker _stepMocker = new ChessValidationStepsMocker();

        [SetUp]
        public void Setup()
        {
            _validator = new KingCastleValidator(_stepMocker.Build());

            _stepMocker.SetupKingCastleEligibility(false);
            _stepMocker.SetupCastleRookEligibility(false);
            _stepMocker.SetupPathIsClear(false);
            _stepMocker.SetupPathIsSafe(false);
        }
        [Test]
        public void ValidateMove_fails_when_king_is_not_allowed_to_castle()
        {
            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_fails_when_rook_is_not_allowed_to_castle()
        {
            _stepMocker.SetupKingCastleEligibility(true);

            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_fails_when_path_between_is_not_clear()
        {
            _stepMocker.SetupKingCastleEligibility(true);
            _stepMocker.SetupCastleRookEligibility(true);

            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_fails_when_path_between_is_not_safe()
        {
            _stepMocker.SetupKingCastleEligibility(true);
            _stepMocker.SetupCastleRookEligibility(true);
            _stepMocker.SetupPathIsClear(true);

            Assert.False(_validator.ValidateMove(null, null));
        }
        [Test]
        public void ValidateMove_succeeeds_when_all_steps_pass()
        {
            _stepMocker.SetupKingCastleEligibility(true);
            _stepMocker.SetupCastleRookEligibility(true);
            _stepMocker.SetupPathIsClear(true);
            _stepMocker.SetupPathIsSafe(true);

            Assert.True(_validator.ValidateMove(null, null));
        }
    }
}