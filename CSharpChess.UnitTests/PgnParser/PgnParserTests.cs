using System.Collections.Generic;
using System.Linq;
using CSharpChess.Pgn;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParser
{
    [TestFixture]
    public class PgnParserTests : PgnParserTestsBase
    {
        // https://en.wikipedia.org/wiki/Portable_Game_Notation
        private const string PgnGame = @"[Event ""F/S Return Match""]
[Site ""Belgrade, Serbia JUG""]
[Date ""1992.11.04""]
[Round ""29""]
[White ""Fischer, Robert J.""]
[Black ""Spassky, Boris V.""]
[Result ""1/2-1/2""]

1. e4 e5 2. Nf3 Nc6 3. Bb5 a6 {This opening is called the Ruy Lopez.}
4. Ba4 Nf6 5. O-O Be7 6. Re1 b5 7. Bb3 d6 8. c3 O-O 9. h3 Nb8 10. d4 Nbd7
11. c4 c6 12. cxb5 axb5 13. Nc3 Bb7 14. Bg5 b4 15. Nb1 h6 16. Bh4 c5 17. dxe5
Nxe4 18. Bxe7 Qxe7 19. exd6 Qf6 20. Nbd2 Nxd6 21. Nc4 Nxc4 22. Bxc4 Nb6
23. Ne5 Rae8 24. Bxf7+ Rxf7 25. Nxf7 Rxe1+ 26. Qxe1 Kxf7 27. Qe3 Qg5 28. Qxg5
hxg5 29. b3 Ke6 30. a3 Kd6 31. axb4 cxb4 32. Ra5 Nd5 33. f3 Bc8 34. Kf2 Bf5
35. Ra7 g6 36. Ra6+ Kc5 37. Ke1 Nf4 38. g3 Nxh3 39. Kd2 Kb5 40. Rd6 Kc5 41. Ra6
Nf2 42. g4 Bd3 43. Re6 1/2-1/2";

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

            AssertPgnMoveQueryIs(turn.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
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
            AssertPgnMoveQueryIs(turn.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");
        }

        [Test]
        public void can_parse_white_pawn_take_pgn_turn()
        {
            var text = "1. dxe4";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            Assert.That(turn.Number, Is.EqualTo(1));
            AssertPgnMoveQueryIs(turn.White, Chess.Board.ChessFile.D, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Take, "E4");
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

            AssertPgnMoveQueryIs(turn.Black, Chess.Board.ChessFile.D, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Take, "E4");
        }

        [Test]
        public void can_parse_paired_pgn_turn()
        {
            var text = "3. e4 e5";

            var pgnTurns = AssertPgnTurnQueryParsed(text);

            Assert.That(pgnTurns.Count(), Is.EqualTo(1));
            var turn = pgnTurns.First();

            AssertPgnMoveQueryIs(turn.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
            AssertPgnMoveQueryIs(turn.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");
        }

        [Test]
        public void can_parse_multiple_paired_pgn_turns()
        {
            var text = "3. e4 e5 4. d4 d5 5. a4 a5";

            var pgnTurns = AssertPgnTurnQueryParsed(text).ToList();

            Assert.That(pgnTurns.Count(), Is.EqualTo(3));

            var first = pgnTurns.First();

            AssertPgnMoveQueryIs(first.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "E4");
            AssertPgnMoveQueryIs(first.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "E5");

            var second = pgnTurns.Skip(1).Take(1).First();
            AssertPgnMoveQueryIs(second.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "D4");
            AssertPgnMoveQueryIs(second.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "D5");

            var third = pgnTurns.Skip(2).Take(1).First();
            AssertPgnMoveQueryIs(third.White, Chess.Board.ChessFile.None, Chess.Colours.White, Chess.PieceNames.Pawn, MoveType.Move, "A4");
            AssertPgnMoveQueryIs(third.Black, Chess.Board.ChessFile.None, Chess.Colours.Black, Chess.PieceNames.Pawn, MoveType.Move, "A5");

        }
    }
}