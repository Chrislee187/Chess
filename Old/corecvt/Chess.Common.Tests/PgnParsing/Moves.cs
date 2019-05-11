using Chess.Common.Tests.Helpers;
using Chess.Common.Tests.Pgn;
using CSharpChess;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.PgnParsing
{
    [TestFixture]
    public class Moves : PgnParserTestsBase
    {

        [Test]
        public void can_parse_basic_pawn_query()
        {
            var move = "e4";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Pawn));

            AssertMoveQueryLocations(pgnQuery, ChessFile.E, 0, ChessFile.E, 4);

            pgnQuery.ResolveQuery(new Board());

            Assert.That(pgnQuery.QueryResolved);
            AssertMoveFromLocation(pgnQuery, ChessFile.E, 2);
        }

        [Test]
        public void can_parse_basic_nonpawn_query()
        {
            var move = "Nc3";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, ChessFile.None, 0, ChessFile.C, 3);

            pgnQuery.ResolveQuery(new Board());

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(ChessFile.B));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(1));
        }

        [Test]
        public void can_parse_basic_kingside_castle()
        {
            var move = "O-O";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.King));
            AssertMoveQueryLocations(pgnQuery, ChessFile.E, 1, ChessFile.G, 1);
        }

        [Test]
        public void can_parse_basic_queenside_castle()
        {
            var move = "O-O-O";
            var turn = Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.Black, PieceNames.King));
            AssertMoveQueryLocations(pgnQuery, ChessFile.E, 8, ChessFile.C, 8);
        }

        [Test]
        public void can_parse_basic_check()
        {
            var move = "Ra6+";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Rook));
            AssertMoveQueryLocations(pgnQuery, ChessFile.None, 0, ChessFile.A, 6);


            const string asOneChar = "........" +
                         "........" +
                         ".....k.." +
                         "........" +
                         "........" +
                         "........" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(ChessFile.A));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(1));
        }

        [Test]
        public void can_parse_disambiguated_file_move()
        {
            var move = "Nfd7";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, ChessFile.F, 0, ChessFile.D, 7);

            const string asOneChar = "........" +
                         "........" +
                         ".N...N.." +
                         "........" +
                         "........" +
                         ".......k" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(ChessFile.F));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(6));
        }

        [TestCase("R8xb3", 8)]
        [TestCase("N4f3", 4)]
        public void can_parse_disambiguated_rank_move2(string move, int rank)
        {
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.FromRank, Is.EqualTo(rank));
        }

        [Test]
        public void can_parse_disambiguated_take()
        {
            var move = "Nfxd7";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, ChessFile.F, 0, ChessFile.D, 7);

            const string asOneChar = "........" +
                         "...b...." +
                         ".N...N.." +
                         "........" +
                         "........" +
                         ".......k" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(ChessFile.F));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(6));
        }
        [Test]
        public void can_parse_named_piece_take()
        {
            var move = "Nxe4";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, ChessFile.None, 0, ChessFile.E, 4);

            const string asOneChar = ".....k.." +
                         "........" +
                         "........" +
                         "........" +
                         "....p..." +
                         "..N....." +
                         "........" +
                         "....K...";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(ChessFile.C));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(3));
        }

        [Test]
        public void can_parse_pawn_take()
        {
            var move = "cxb5";
            var turn = Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.White, PieceNames.Pawn));
            AssertMoveQueryLocations(pgnQuery, ChessFile.C, 0, ChessFile.B, 5);

            const string asOneChar = ".....k.." +
                         "........" +
                         "........" +
                         ".p......" +
                         "..P....." +
                         "........" +
                         "........" +
                         "....K...";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(ChessFile.C));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(4));
        }

        //c1=Q
        [Test]
        public void can_handle_promotions()
        {
            var move = "h1=Q+";
            var turn = Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.Black, PieceNames.Pawn));
            Assert.That(pgnQuery.PromotionPiece, Is.EqualTo(PieceNames.Queen));
            AssertMoveQueryLocations(pgnQuery, ChessFile.H, 0, ChessFile.H, 1);
        }

        //fxe1=Q+
        [Test]
        public void can_parse_complex_notations()
        {
            var move = "fxe1=Q+";
            var turn = Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.Black, PieceNames.Pawn));
            AssertMoveQueryLocations(pgnQuery, ChessFile.F, 0, ChessFile.E, 1);
        }

        //fxe1=Q+
        [Test]
        public void can_parse_result()
        {
            var move = "1-0";
            var turn = Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Colours.None, PieceNames.Blank));
            AssertMoveQueryLocations(pgnQuery, ChessFile.None, 0, ChessFile.None, 0);
            Assert.That(pgnQuery.GameOver, Is.True);
            Assert.That(pgnQuery.GameResult, Is.EqualTo(ChessGameResult.WhiteWins));
        }
    }
}
