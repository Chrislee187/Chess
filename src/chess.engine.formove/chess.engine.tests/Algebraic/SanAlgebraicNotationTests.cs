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
        [TestCase("d1=Q+", ChessPieceName.Queen)]
        public void ShouldParsePromotion(string notation, ChessPieceName piece)
        {
            Assert.That(StandardAlgebraicNotation.TryParse(notation, out var an));

            Assert.That(an.PromotionPiece.HasValue);
            Assert.That(an.PromotionPiece.Value, Is.EqualTo(piece));
        }

        [TestCase("x8=Q")]
        [TestCase("DD4xe8")]
        [TestCase("e8=Z")]
        public void ShouldFailParsing(string notation)
        {
            Assert.False(StandardAlgebraicNotation.TryParse(notation, out _));
            Assert.That(() => StandardAlgebraicNotation.Parse(notation), Throws.Exception);
        }

        [TestCase("a2", "a3", DefaultActions.MoveOnly, "a3")]
        [TestCase("b1", "a3", DefaultActions.MoveOrTake, "Na3")]
        public void ShouldParseFromBoardMove(string @from, string to, DefaultActions moveType, string expectedSan)
        {
            var game = ChessFactory.NewChessGame();
            var move = BoardMove.Create(from.ToBoardLocation(), to.ToBoardLocation(), (int)moveType);

            Assert.That(StandardAlgebraicNotation.ParseFromGameMove(game.BoardState, move).ToNotation(), Is.EqualTo(expectedSan));
        }

        [TestCase("R4h2", "h4h2")]
        public void ShouldDisambiguateRank(string move, string expected)
        {
            var san = StandardAlgebraicNotation.Parse(move);
            Assert.False(san.FromFileX.HasValue);
            Assert.True(san.FromRankY.HasValue);
            Assert.That(san.FromRankY.Value, Is.EqualTo(4));

        }

        [TestCase("Rxf6")]
        [TestCase("R4h2")]
        [TestCase("Bab2")]
        [TestCase("Bhxg7")]
        [TestCase("axb4+")]
        public void Should_parse_san_notation(string san)
        {
            Assert.That(StandardAlgebraicNotation.Parse(san).ToNotation(), Is.EqualTo(san));
        }

        [Test]
        public void Should_put_plus_on_end_of_moves_that_cause_check()
        {
            // TODO: Better way to check this, than using a full board.
            var builder = new ChessBoardBuilder()
                .Board("....rrk." +
                       ".b...pp." +
                       ".n...q.p" +
                       "..p.N..." +
                       ".pB....." +
                       ".......P" +
                       "PP...PP." +
                       "R..QR.K."
                );

            var game = ChessFactory.CustomChessGame(builder.ToGameSetup());
            var from = "C4".ToBoardLocation();
            var piece = game.BoardState.GetItem(from);
            var boardMove = piece.Paths.FindMove(from, "f7".ToBoardLocation());
            var san = StandardAlgebraicNotation.ParseFromGameMove(game.BoardState, boardMove, true);
            Assert.That(san.ToNotation(), Is.EqualTo("Bxf7+"));
        }


    }

}