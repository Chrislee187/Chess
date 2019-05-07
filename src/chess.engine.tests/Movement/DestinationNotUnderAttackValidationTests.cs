using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;
using chess.engine.Movement.Validators;
using chess.engine.tests.Chess.Movement.King;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class DestinationNotUnderAttackValidationTests : ValidatorTestsBase
    {
        private IBoardState<ChessPieceEntity> _boardState;
        private DestinationNotUnderAttackValidator<ChessPieceEntity> _validator;

        [SetUp]
        public void SetUp()
        {
            var board = new EasyBoardBuilder()
                .Board("r  qk  r" +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "R   K  R"
                );
            var game = new ChessGame(NullLogger<ChessGame>.Instance, ChessBoardEngineProvider, board.ToGameSetup());
            _boardState = game.BoardState;
            _validator = new DestinationNotUnderAttackValidator<ChessPieceEntity>();
        }

        [Test]
        public void Should_return_false_for_square_under_enemy_attack()
        {
            var containsEnemy = BoardMove.Create("E1", "D1", MoveType.MoveOrTake);
            Assert.False(_validator.ValidateMove(containsEnemy, _boardState));

        }
        [Test]
        public void Should_return_true_for_square_under_friendly_attack()
        {
            var noEnemy = BoardMove.Create("E1", "F1", MoveType.MoveOrTake);
            Assert.True(_validator.ValidateMove(noEnemy, _boardState));

        }
        [Test]
        public void Should_return_true_for_square_under_no_attack()
        {
            var noEnemy = BoardMove.Create("E1", "E2", MoveType.MoveOrTake);
            Assert.True(_validator.ValidateMove(noEnemy, _boardState));

        }
    }
}