using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using chess.engine.tests.Chess.Movement.King;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class UpdatePieceValidationTests : ValidatorTestsBase
    {
        private IBoardState<ChessPieceEntity> _boardState;

        [SetUp]
        public void SetUp()
        {
            var board = new ChessBoardBuilder()
                .Board("   qk  r" +
                       "P       " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K  R"
                );
            var game = new ChessGame(NullLogger<ChessGame>.Instance, ChessBoardEngineProvider, ChessBoardEntityFactory, ChessGameStateService, board.ToGameSetup());
            _boardState = game.BoardState;
        }

        [Test]
        public void Should_return_true_for_valid_promotion()
        {
            var validator = new UpdatePieceValidator<ChessPieceEntity>();

            BoardLocation to = "A8".ToBoardLocation();
            var promote = new BoardMove("A7".ToBoardLocation(), to, (int)DefaultActions.UpdatePiece, new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeData
            {
                Owner = 0,
                PieceName = ChessPieceName.Queen
            });
            Assert.True(validator.ValidateMove(promote, _boardState));
        }
    }
}