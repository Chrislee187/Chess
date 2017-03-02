using System.Collections.Generic;
using System.Linq;
using CsChess.Pgn;
using CSharpChess.System;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class Moves : PgnParserTestsBase
    {

        [Test]
        public void can_parse_basic_pawn_query()
        {
            var move = "e4";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Pawn));

            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.E, 0, Chess.Board.ChessFile.E, 4);

            pgnQuery.ResolveQuery(new ChessBoard());

            Assert.That(pgnQuery.QueryResolved);
            AssertMoveFromLocation(pgnQuery, Chess.Board.ChessFile.E, 2);
        }

        [Test]
        public void can_parse_basic_nonpawn_query()
        {
            var move = "Nc3";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.C, 3);

            pgnQuery.ResolveQuery(new ChessBoard());

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.B));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(1));
        }

        [Test]
        public void can_parse_basic_kingside_castle()
        {
            var move = "O-O";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.King));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.E, 1, Chess.Board.ChessFile.G, 1);
        }

        [Test]
        public void can_parse_basic_queenside_castle()
        {
            var move = "O-O-O";
            var turn = Chess.Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.Black, Chess.PieceNames.King));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.E, 8, Chess.Board.ChessFile.C, 8);
        }

        [Test]
        public void can_parse_basic_check()
        {
            var move = "Ra6+";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Rook));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.A, 6);


            const string asOneChar = "........" +
                         "........" +
                         ".....k.." +
                         "........" +
                         "........" +
                         "........" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.A));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(1));
        }

        [Test]
        public void can_parse_disambiguated_file_move()
        {
            var move = "Nfd7";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.F, 0, Chess.Board.ChessFile.D, 7);

            const string asOneChar = "........" +
                         "........" +
                         ".N...N.." +
                         "........" +
                         "........" +
                         ".......k" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.F));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(6));
        }

        [Test]
        public void can_parse_disambiguated_rank_move()
        {
            var move = "N4f3";
//            var move = "Rhe8";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.None, 4, Chess.Board.ChessFile.F, 3);

            const string asOneChar = 
                         "r...kb.r" +
                         "pb...ppp" +
                         "..p..n.." +
                         "........" +
                         "..PN.B.." +
                         "......P." +
                         "PP.NP..P" +
                         "R...K..R";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.D));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(4));
        }

        [Test]
        public void can_parse_disambiguated_take()
        {
            var move = "Nfxd7";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.F, 0, Chess.Board.ChessFile.D, 7);

            const string asOneChar = "........" +
                         "...b...." +
                         ".N...N.." +
                         "........" +
                         "........" +
                         ".......k" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.F));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(6));
        }
        [Test]
        public void can_parse_named_piece_take()
        {
            var move = "Nxe4";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.E, 4);

            const string asOneChar = ".....k.." +
                         "........" +
                         "........" +
                         "........" +
                         "....p..." +
                         "..N....." +
                         "........" +
                         "....K...";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.C));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(3));
        }

        [Test]
        public void can_parse_pawn_take()
        {
            var move = "cxb5";
            var turn = Chess.Colours.White;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Pawn));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.C, 0, Chess.Board.ChessFile.B, 5);

            const string asOneChar = ".....k.." +
                         "........" +
                         "........" +
                         ".p......" +
                         "..P....." +
                         "........" +
                         "........" +
                         "....K...";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            pgnQuery.ResolveQuery(customBoard);

            Assert.That(pgnQuery.QueryResolved);
            Assert.That(pgnQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.C));
            Assert.That(pgnQuery.FromRank, Is.EqualTo(4));
        }

        //c1=Q
        [Test]
        public void can_handle_promotions()
        {
            var move = "c1=Q";
            var turn = Chess.Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.Black, Chess.PieceNames.Pawn));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.C, 0, Chess.Board.ChessFile.C, 1);
        }

        //fxe1=Q+
        [Test]
        public void can_parse_complex_notations()
        {
            var move = "fxe1=Q+";
            var turn = Chess.Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(!pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.Black, Chess.PieceNames.Pawn));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.F, 0, Chess.Board.ChessFile.E, 1);
        }

        //fxe1=Q+
        [Test]
        public void can_parse_result()
        {
            var move = "1-0";
            var turn = Chess.Colours.Black;
            var pgnQuery = new PgnQuery();
            var parsed = PgnMoveParser.TryParse(turn, move, ref pgnQuery);

            Assert.True(parsed);
            Assert.That(pgnQuery.QueryResolved);
            Assert.True(pgnQuery.Piece.Is(Chess.Colours.None, Chess.PieceNames.Blank));
            AssertMoveQueryLocations(pgnQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.None, 0);
            Assert.That(pgnQuery.GameOver, Is.True);
            Assert.That(pgnQuery.GameResult, Is.EqualTo(ChessGameResult.WhiteWins));
        }
    }
}
