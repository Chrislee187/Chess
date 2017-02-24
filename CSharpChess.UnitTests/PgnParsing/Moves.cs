using System.Collections.Generic;
using System.Linq;
using CSharpChess.Pgn;
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
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(!moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Pawn));

            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.E, 0, Chess.Board.ChessFile.E, 4);

            moveQuery.ResolveWithBoard(new ChessBoard());

            Assert.That(moveQuery.QueryResolved);
            AssertMoveFromLocation(moveQuery, Chess.Board.ChessFile.E, 2);
        }

        [Test]
        public void can_parse_basic_nonpawn_query()
        {
            var move = "Nc3";
            var turn = Chess.Colours.White;
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(!moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.C, 3);

            moveQuery.ResolveWithBoard(new ChessBoard());

            Assert.That(moveQuery.QueryResolved);
            Assert.That(moveQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.B));
            Assert.That(moveQuery.FromRank, Is.EqualTo(1));
        }

        [Test]
        public void can_parse_basic_kingside_castle()
        {
            var move = "O-O";
            var turn = Chess.Colours.White;
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.King));
            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.E, 1, Chess.Board.ChessFile.G, 1);
        }

        [Test]
        public void can_parse_basic_check()
        {
            var move = "Ra6+";
            var turn = Chess.Colours.White;
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(!moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Rook));
            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.A, 6);


            const string asOneChar = "........" +
                         "........" +
                         ".....k.." +
                         "........" +
                         "........" +
                         "........" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            moveQuery.ResolveWithBoard(customBoard);

            Assert.That(moveQuery.QueryResolved);
            Assert.That(moveQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.A));
            Assert.That(moveQuery.FromRank, Is.EqualTo(1));
        }

        [Test]
        public void can_parse_disambiguated_move()
        {
            var move = "Nfd7";
            var turn = Chess.Colours.White;
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(!moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.F, 0, Chess.Board.ChessFile.D, 7);

            const string asOneChar = "........" +
                         "........" +
                         ".N...N.." +
                         "........" +
                         "........" +
                         ".......k" +
                         ".K......" +
                         "R.......";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            moveQuery.ResolveWithBoard(customBoard);

            Assert.That(moveQuery.QueryResolved);
            Assert.That(moveQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.F));
            Assert.That(moveQuery.FromRank, Is.EqualTo(6));
        }

        [Test]
        public void can_parse_named_piece_take()
        {
            var move = "Nxe4";
            var turn = Chess.Colours.White;
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(!moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight));
            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.None, 0, Chess.Board.ChessFile.E, 4);

            const string asOneChar = ".....k.." +
                         "........" +
                         "........" +
                         "........" +
                         "....p..." +
                         "..N....." +
                         "........" +
                         "....K...";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            moveQuery.ResolveWithBoard(customBoard);

            Assert.That(moveQuery.QueryResolved);
            Assert.That(moveQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.C));
            Assert.That(moveQuery.FromRank, Is.EqualTo(3));
        }

        [Test]
        public void can_parse_pawn_take()
        {
            var move = "cxb5";
            var turn = Chess.Colours.White;
            MoveQuery moveQuery;
            var parsed = PgnMoveQuery.TryParse(turn, move, out moveQuery);

            Assert.True(parsed);
            Assert.That(!moveQuery.QueryResolved);
            Assert.True(moveQuery.Piece.Is(Chess.Colours.White, Chess.PieceNames.Pawn));
            AssertMoveQueryLocations(moveQuery, Chess.Board.ChessFile.C, 0, Chess.Board.ChessFile.B, 5);

            const string asOneChar = ".....k.." +
                         "........" +
                         "........" +
                         ".p......" +
                         "..P....." +
                         "........" +
                         "........" +
                         "....K...";

            var customBoard = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            moveQuery.ResolveWithBoard(customBoard);

            Assert.That(moveQuery.QueryResolved);
            Assert.That(moveQuery.FromFile, Is.EqualTo(Chess.Board.ChessFile.C));
            Assert.That(moveQuery.FromRank, Is.EqualTo(4));
        }
    }
}