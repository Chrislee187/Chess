using System.Collections.Generic;
using System.Linq;
using CSharpChess.Pgn;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class Moves : PgnParserTestsBase
    {
        [TestCase("", Chess.PieceNames.Pawn)]
        [TestCase("P", Chess.PieceNames.Pawn)]
        [TestCase("N", Chess.PieceNames.Knight)]
        [TestCase("B", Chess.PieceNames.Bishop)]
        [TestCase("R", Chess.PieceNames.Rook)]
        [TestCase("Q", Chess.PieceNames.Queen)]
        [TestCase("K", Chess.PieceNames.King)]
        public void can_parse_basic_pieces(string pieceChar, Chess.PieceNames pieceName)
        {
            var move = $"{pieceChar}f3";
            var turn = Chess.Colours.White;
            PgnMoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed, "Did not parse!");
            Assert.That(moveQuery.Piece, Is.EqualTo(new ChessPiece(Chess.Colours.White, pieceName)));
        }

        [Test]
        public void can_parse_basic_pawn_query()
        {
            var move = "e4";
            var turn = Chess.Colours.White;
            PgnMoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Pawn));
            Assert.That(moveQuery.Destination, Is.EqualTo(BoardLocation.At("e4")));
            Assert.That(moveQuery.MoveType, Is.EqualTo(MoveType.Move));
        }

        [Test]
        public void can_parse_white_only_pgn_turn()
        {
            var text = "1. e4";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();
            Assert.That(turn.Number, Is.EqualTo(1));

            AssertPgnMoveQueryIs(turn.White, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
        }

        [Test]
        public void can_parse_black_only_pgn_turn()
        {
            var text = "1... e5";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();
            Assert.That(turn.Number, Is.EqualTo(1));

            Assert.IsNull(turn.White);
            AssertPgnMoveQueryIs(turn.Black, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");
        }

        [Test]
        public void can_parse_white_pawn_take_pgn_turn()
        {
            var text = "1. dxe4";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            Assert.That(turn.Number, Is.EqualTo(1));
            AssertPgnMoveQueryIs(turn.White, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Take, "E4", Chess.Board.ChessFile.D);
        }

        [Test]
        public void can_parse_black_pawn_take_pgn_turn()
        {
            var text = "2... dxe4";

            IEnumerable<PgnTurnQuery> pgnTurns;
            var parsed = PgnTurnsParser.TryParse(text, out pgnTurns);

            Assert.True(parsed);
            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.Black, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Take, "E4", Chess.Board.ChessFile.D);
        }

        [Test]
        public void can_parse_paired_pgn_turn()
        {
            var text = "3. e4 e5";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.White, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
            AssertPgnMoveQueryIs(turn.Black, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");
        }

        [Test]
        public void can_parse_multiple_paired_pgn_turns()
        {
            var text = "3. e4 e5 4. d4 d5 5. a4 a5";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(3));

            var first = pgnTurns.First();

            AssertPgnMoveQueryIs(first.White, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
            AssertPgnMoveQueryIs(first.Black, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");

            var second = pgnTurns.Skip(1).Take(1).First();
            AssertPgnMoveQueryIs(second.White, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "D4");
            AssertPgnMoveQueryIs(second.Black, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "D5");

            var third = pgnTurns.Skip(2).Take(1).First();
            AssertPgnMoveQueryIs(third.White, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "A4");
            AssertPgnMoveQueryIs(third.Black, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "A5");

        }

        [TestCase("O-O", "C1")]
        [TestCase("O-O-O", "G1")]
        public void can_parse_castle(string move, string kingDestination)
        {
            var turn = Chess.Colours.White;
            PgnMoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.King));
            Assert.That(moveQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.E));
            Assert.That(moveQuery.Destination, Is.EqualTo(BoardLocation.At(kingDestination)));
            Assert.That(moveQuery.MoveType, Is.EqualTo(MoveType.Castle));
        }

        [Test]
        public void can_parse_basic_pieces_with_start_file()
        {
            var move = $"Nbd7";
            var turn = Chess.Colours.White;
            PgnMoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed, "Did not parse!");
            AssertPgnMoveQueryIs(moveQuery, Chess.Colours.White, Chess.PieceNames.Knight, MoveType.Move, "D7", Chess.Board.ChessFile.B);
        }

    }
}