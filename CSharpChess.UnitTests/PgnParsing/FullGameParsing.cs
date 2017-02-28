using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CsChess.Pgn;
using CSharpChess.System;
using NUnit.Framework;

namespace CSharpChess.UnitTests.PgnParsing
{
    [TestFixture]
    public class FullGameParsing
    {
        #region Test Games
        // https://en.wikipedia.org/wiki/Portable_Game_Notation
        private const string WikiGame = @"
[Event ""F/S Return Match""]
[Site ""Belgrade, Serbia JUG""]
[Date ""1992.11.04""]
[Round ""29""]
[White ""Fischer, Robert J.""]
[Black ""Spassky, Boris V.""]
[Result ""1/2-1/2""]
        
1. e4 e5 2. Nf3 Nc6 3. Bb5 a6 
4. Ba4 Nf6 5. O-O Be7 6. Re1 b5 7. Bb3 d6 8. c3 O-O 9. h3 Nb8 10. d4 Nbd7
11. c4 c6 12. cxb5 axb5 13. Nc3 Bb7 14. Bg5 b4 15. Nb1 h6 16. Bh4 c5 17. dxe5
Nxe4 18. Bxe7 Qxe7 19. exd6 Qf6 20. Nbd2 Nxd6 21. Nc4 Nxc4 22. Bxc4 Nb6
23. Ne5 Rae8 24. Bxf7+ Rxf7 25. Nxf7 Rxe1+ 26. Qxe1 Kxf7 27. Qe3 Qg5 28. Qxg5
hxg5 29. b3 Ke6 30. a3 Kd6 31. axb4 cxb4 32. Ra5 Nd5 33. f3 Bc8 34. Kf2 Bf5
35. Ra7 g6 36. Ra6+ Kc5 37. Ke1 Nf4 38. g3 Nxh3 39. Kd2 Kb5 40. Rd6 Kc5 41. Ra6
Nf2 42. g4 Bd3 43. Re6 1/2-1/2
";

        private const string ShortGames = @"
[Event ""Staffordshire op""]
[Site ""Staffordshire""]
[Date ""1975.??.??""]
[Round ""5""]
[White ""Short, Nigel D""]
[Black ""Evans, JS.""]
[Result ""1-0""]
[WhiteElo """"]
[BlackElo """"]
[ECO ""B02""]

1.e4 d5 2.exd5 Nf6 3.Nc3 Nxd5 4.Nxd5 Qxd5 5.d4 Nc6 6.Be3 e5 7.dxe5 Qxe5 8.c3 Bd6
9.Nf3 Bg4 10.Nxe5 Bxd1 11.Nxc6 Ba4 12.Nd4 Bd7 13.Nb5 Be5 14.f4 a6 15.Nd4 Bd6
16.O-O-O O-O 17.Bd3 Rae8 18.Rhe1 Bg4 19.Be2 Rxe3 20.Bxg4 Rxe1 21.Rxe1 Bxf4+
22.Kc2 Bxh2 23.Bf3 c5 24.Nf5 b6 25.Ne7+ Kh8 26.Nd5 b5 27.Re7 f5 28.Ra7 g5
29.Rxa6 g4 30.Be2 Rd8 31.Rh6 Bg1 32.Rh1 Bf2 33.Rd1 c4 34.b3 Ra8 35.Kb2 Re8
36.Bf1 Re4 37.bxc4 bxc4 38.Nf6 Re6 39.Nh5 Rb6+ 40.Kc2 h6 41.Bxc4 Kh7 42.Nf4 Kg7
43.Rd7+ Kh8 44.a4 Rc6 45.Bd5 Rf6 46.Kb3 Be3 47.Rd8+ Kh7 48.Bg8+ Kh8 49.Nh5 Bb6
50.Rc8 Rd6 51.Be6+ Bd8 52.Bxf5 Kg8 53.Nf6+ Kf7 54.Nxg4 Bb6 55.Kb4 h5 56.Nh2 Rd2
57.Be4 Rb2+ 58.Kc4 Ke7 59.Rh8 Ra2 60.Kb3 Re2 61.Bf3 Re3 62.Nf1 Re1 63.Nd2 Bc7
64.Rxh5 Bf4 65.Ne4 Rb1+ 66.Ka2 Rb8 67.a5 Bc1 68.g4 Rb2+ 69.Ka1 Rb3 70.Bd1 Ra3+
71.Kb1 Be3 72.Kb2  1-0

[Event ""BBC TV Master Game""]
[Site ""?""]
[Date ""1976.??.??""]
[Round ""1""]
[White ""Short, Nigel D""]
[Black ""Hartston, William R""]
[Result ""0-1""]
[WhiteElo """"]
[BlackElo ""2445""]
[ECO ""E01""]

1.d4 Nf6 2.c4 c5 3.Nf3 cxd4 4.Nxd4 e6 5.g3 d5 6.Bg2 e5 7.Nc2 d4 8.f4 exf4
9.Bxf4 Nc6 10.Bxc6+ bxc6 11.Qxd4 Qxd4 12.Nxd4 Bb7 13.Nd2 O-O-O 14.N4f3 Bb4
15.O-O-O Rhe8 16.Ne5 Bxd2+ 17.Rxd2 Rxd2 18.Kxd2 g5 19.Nxf7 Ne4+  0-1

[Event ""BCF-ch""]
[Site ""Brighton""]
[Date ""1977.08.08""]
[Round ""1""]
[White ""Short, Nigel D""]
[Black ""Neat, Kenneth P""]
[Result ""1/2-1/2""]
[WhiteElo """"]
[BlackElo ""2310""]
[ECO ""C02""]

1.e4 c5 2.Nf3 e6 3.c3 d5 4.e5 Nc6 5.d4 Bd7 6.Bd3 Rc8 7.O-O cxd4 8.cxd4 Nb4
9.Nc3 Nxd3 10.Qxd3 Ne7 11.a3 Qb6 12.b4 Rc4 13.Be3 Nf5 14.Nd2 Rc8 15.g4 Nxe3
16.fxe3 Be7 17.e4 dxe4 18.Ndxe4 Bc6 19.Rad1 Rd8 20.Rf4 Rd7 21.g5 Qd8 22.Qg3 a5
23.b5 Bxe4 24.Nxe4 Qb6 25.Qd3 Rd5 26.Nc3 Rd7 27.Rg4 Qd8 28.Ne4 Rd5 29.Kh1 b6
30.a4 Qa8 31.Qc4 O-O 32.Qd3 Rfd8 33.Kg1 Rxe5 34.Nf6+ Bxf6 35.gxf6 g6 36.dxe5 Rxd3
37.Rxd3 h5 38.Rc4 Qf8 39.h4 Kh7 40.Kh1 g5 41.hxg5 Kg6 42.Rd6 Kxg5 43.Rxb6 Qa3
44.Rbc6 Qf3+ 45.Kh2 Qf2+ 46.Kh3 Qf1+ 47.Kh2 h4 48.b6 Qe2+ 49.Kh1 Qe1+ 50.Kg2 Qe2+  1/2-1/2

";
#endregion

        [Test]
        public void can_parse_wiki_sample_game()
        {
            var pgnGameText = WikiGame;

            var pgnGame = PgnGame.Parse(pgnGameText).First();

            Assert.That(pgnGame.Event, Is.EqualTo("F/S Return Match"));
            Assert.That(pgnGame.Site, Is.EqualTo("Belgrade, Serbia JUG"));
            Assert.That(pgnGame.Date.ToString(), Is.EqualTo("1992.11.04"));
            Assert.That(pgnGame.Round, Is.EqualTo(29));
            Assert.That(pgnGame.White, Is.EqualTo("Fischer, Robert J."));
            Assert.That(pgnGame.Black, Is.EqualTo("Spassky, Boris V."));
            Assert.That(pgnGame.Result, Is.EqualTo(ChessGameResult.Draw));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));

            Assert.That(pgnGame.TurnQueries.Count(), Is.EqualTo(43));
        }

        [Test]
        public void can_parse_multiple_games()
        {
            var pgnText = ShortGames;

            var pgnGames = PgnGame.Parse(pgnText);

            Assert.That(pgnGames.Count(), Is.EqualTo(3));

        }


        [Test]
        public void can_parse_lots_of_games()
        {
            
            var pgnText = File.ReadAllText(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\short.pgn");

            var pgnGames = PgnGame.Parse(pgnText);

            Assert.That(pgnGames.Count(), Is.GreaterThan(0));
            Console.WriteLine($"{pgnGames.Count()} parsed.");

        }


        [Test]
        public void can_read_pgn_text()
        {

            var stream = File.OpenRead(@"C:\Src\Chess\CSharpChess.UnitTests\bin\Debug\short.pgn");

            int count = 0;

            using (var reader = new PgnReader(stream))
            {
                var game = reader.ReadGame();

                while (game != null)
                {
                    count++;
                    game = reader.ReadGame();
                }
            }

            Console.WriteLine(count);

        }

    }
}
