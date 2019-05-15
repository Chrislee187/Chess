using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Algebraic;
using chess.engine.Chess;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.tests.Algebraic
{
    [TestFixture]
    public class AlgebraicNotationTests
    {
        [TestCase("Re6", ChessPieceName.Rook)]
        [TestCase("Ne6", ChessPieceName.Knight)]
        [TestCase("Be6", ChessPieceName.Bishop)]
        [TestCase("Qe6", ChessPieceName.Queen)]
        [TestCase("Ke6", ChessPieceName.King)]
        [TestCase("e1", ChessPieceName.Pawn)]
        [TestCase("O-O", ChessPieceName.King)]
        [TestCase("O-O-O", ChessPieceName.King)]
        [TestCase("0-0", ChessPieceName.King)]
        [TestCase("0-0-0", ChessPieceName.King)]
        public void ShouldParsePieceName(string notation, ChessPieceName piece)
        {
            StandardAlgebraicNotation.Parse(notation);

            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.Piece, Is.EqualTo(piece));
        }

        [TestCase("Rxe6", ChessPieceName.Rook)]
        public void ShouldParsePieceNameFromComplexNotations(string notation, ChessPieceName piece)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.Piece, Is.EqualTo(piece));
        }

        [TestCase("Rxe6")]
        public void ShouldParseTakeMoves(string notation)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.MoveType, Is.EqualTo(SanMoveTypes.Take));
        }

        [TestCase("Re6", 5)]
        [TestCase("Rxe6", 5)]
        public void ShouldParseToFile(string notation, int file)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.ToFileX, Is.EqualTo(file));
        }

        [TestCase("Re6", 6)]
        [TestCase("Rxe6", 6)]
        [TestCase("Ra6e6", 6)]
        [TestCase("Ra6xe6", 6)]
        public void ShouldParseToRank(string notation, int rank)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.ToRankY, Is.EqualTo(rank));
        }

        [TestCase("Rae6", 1)]
        [TestCase("Raxe6", 1)]
        public void ShouldParseFromFile(string notation, int file)
        {
            StandardAlgebraicNotation.Parse(notation);
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.True(an.FromFileX.HasValue);
            Assert.That(an.FromFileX.Value, Is.EqualTo(file));
        }

        [TestCase("Ra6e6", 1)]
        [TestCase("Ra6xe6", 1)]
        public void ShouldParseFromRank(string notation, int rank)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.True(an.FromFileX.HasValue);
            Assert.That(an.FromFileX.Value, Is.EqualTo(rank));
        }
        [TestCase("e8=Q", ChessPieceName.Queen)]
        [TestCase("dxe8=Q", ChessPieceName.Queen)]
        [TestCase("d7xe8=Q", ChessPieceName.Queen)]
        public void ShouldParsePromotion(string notation, ChessPieceName piece)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.PromotionPiece.HasValue);
            Assert.That(an.PromotionPiece.Value, Is.EqualTo(piece));
        }

        [TestCase("x8=Q")]
        [TestCase("DD4xe8")]
        [TestCase("e8+Z")]
        public void ShouldFailParsing(string notation)
        {
            Assert.False(StandardAlgebraicNotation.TryParse(notation, out var an));
        }

        [TestCase("a2", "a3", DefaultActions.MoveOnly, "a3")]
        [TestCase("b1", "a3", DefaultActions.MoveOrTake, "Na3")]
        public void ShouldParseFromBoardMove(string @from, string to, DefaultActions moveType, string expectedSan)
        {
            var game = ChessFactory.NewChessGame();
            var move = BoardMove.Create(from.ToBoardLocation(), to.ToBoardLocation(), (int) moveType);

            Assert.That(StandardAlgebraicNotation.ParseFromGameMove(game.BoardState, move).ToNotation(), Is.EqualTo(expectedSan));
        }

        [TestCase("A1","B2", DefaultActions.MoveOrTake, "Bab2")]
        [TestCase("H8","G7", DefaultActions.MoveOrTake, "Bhxg7")]
        [TestCase("A3","B4", DefaultActions.TakeOnly, "axb4")]
        public void ShouldDisambiguateFile(string from, string to, int moveType, string expectedNotation)
        {
            var builder = new ChessBoardBuilder()
                .Board("r   kb b" +
                       "      P " +
                       "        " +
                       "        " +
                       " p      " +
                       "P       " +
                       "        " +
                       "B B K  R"
                );

            var game = ChessFactory.CustomChessGame(builder.ToGameSetup(), Colours.White);
            var move = BoardMove.Create(from.ToBoardLocation(), to.ToBoardLocation(), moveType);

            Assert.That(StandardAlgebraicNotation.ParseFromGameMove(game.BoardState, move).ToNotation(), Is.EqualTo(expectedNotation));
        }


    }

}