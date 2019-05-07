using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class ChessGameTests
    {
        private IRefreshAllPaths<ChessPieceEntity> _chessRefreshAllPaths;

        [SetUp]
        public void Setup()
        {
            _chessRefreshAllPaths = new Mock<IRefreshAllPaths<ChessPieceEntity>>().Object;
        }
        [Test]
        public void New_game_should_have_white_as_first_played()
        {
            IMoveValidationFactory<ChessPieceEntity> moveValidationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(_chessRefreshAllPaths, new ChessPathsValidator(new ChessPathValidator(moveValidationFactory)));
            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.White));
        }

        [Test]
        public void Move_should_update_current_player_when_valid()
        {
            IMoveValidationFactory<ChessPieceEntity> moveValidationFactory = new MoveValidationFactory<ChessPieceEntity>();
            var game = new ChessGame(_chessRefreshAllPaths, new ChessPathsValidator(new ChessPathValidator(moveValidationFactory)));

            game.Move("D2D4");
            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.Black));
        }

        [TestCase(Colours.White, 8)]
        [TestCase(Colours.Black, 1)]
        public void EndRankFor_is_correct_for_both_colours(Colours colour, int rank)
        {
            Assert.That(ChessGame.EndRankFor(colour), Is.EqualTo(rank), $"{colour}");
        }

        [Test]
        public void GameState_some_checks_for_this()
        {
            Assert.Inconclusive();

        }
    }
}