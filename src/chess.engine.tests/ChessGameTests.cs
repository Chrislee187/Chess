using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class ChessGameTests
    {
        private IRefreshAllPaths<ChessPieceEntity> _chessRefreshAllPaths;
        private ChessBoardEngineProvider _engineProvider;

        [SetUp]
        public void Setup()
        {
            _chessRefreshAllPaths = new Mock<IRefreshAllPaths<ChessPieceEntity>>().Object;

            _engineProvider = new ChessBoardEngineProvider(NullLogger<BoardEngine<ChessPieceEntity>>.Instance,
                _chessRefreshAllPaths,
                new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>()))
            );
        }
        [Test]
        public void New_game_should_have_white_as_first_played()
        {
            var game = new ChessGame(NullLogger<ChessGame>.Instance, _engineProvider);
            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.White));
        }

        [Test]
        public void Move_should_update_current_player_when_valid()
        {
            var engineProvider = new ChessBoardEngineProvider(
                NullLogger<BoardEngine<ChessPieceEntity>>.Instance,
                new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance),
                new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>()))
            );
            var game = new ChessGame(NullLogger<ChessGame>.Instance, engineProvider);

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