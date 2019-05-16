using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using chess.engine.tests.Builders;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class ChessGameTests
    {
        private ChessBoardEngineProvider _engineProvider;
        private ChessPieceEntityProvider _chessPieceEntityProvider;
        private IPlayerStateService _playerStateService;

        [SetUp]
        public void Setup()
        {
            _chessPieceEntityProvider = ChessFactory.ChessPieceEntityProvider();
            _engineProvider = ChessFactory.ChessBoardEngineProvider();
            _playerStateService = ChessFactory.ChessGameStateService(ChessFactory.LoggerType.Null);
        }
        [Test]
        public void New_game_should_have_white_as_first_played()
        {
            var game = new ChessGame(
                NullLogger<ChessGame>.Instance, 
                _engineProvider, 
                _chessPieceEntityProvider, _playerStateService);
            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.White));
        }

        [Test]
        public void Move_should_update_current_player_when_valid()
        {
            var game = new ChessGame(NullLogger<ChessGame>.Instance, _engineProvider, _chessPieceEntityProvider, _playerStateService);

            var msg = game.Move("d2d4");
            Assert.That(game.CurrentPlayer, Is.EqualTo(Colours.Black), msg);
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