using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.SAN;
using NUnit.Framework;

namespace chess.engine.tests.Algebraic
{
    [TestFixture]
    public class SanAlgebraicNotationTests
    {
        [TestCase("a2", "a3", DefaultActions.MoveOnly, "a3")]
        [TestCase("b1", "a3", DefaultActions.MoveOrTake, "Na3")]
        public void ShouldParseFromBoardMove(string @from, string to, DefaultActions moveType, string expectedSan)
        {
            var game = ChessFactory.NewChessGame();
            var move = BoardMove.Create(from.ToBoardLocation(), to.ToBoardLocation(), (int)moveType);

            Assert.That(StandardAlgebraicNotation.ParseFromGameMove(game.BoardState, move).ToNotation(), Is.EqualTo(expectedSan));
        }
    }

}
