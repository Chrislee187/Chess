using System;
using System.Collections.Generic;
using System.Linq;
using CsChess.Pgn;
using CSharpChess;
using Newtonsoft.Json;

namespace pgn2json
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = @"[Event ""F/S Return Match""]
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

            var pgnGame = PgnGame.Parse(game).Single();
            var board = new PgnGameResolver().Resolve(pgnGame);

            var json = new PgnJson(pgnGame, board);
            Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented));
            Console.ReadLine();
        }
    }

    public class PgnJson
    {
        public PgnJson(PgnGame pgnGame, Board board)
        {
            Event = pgnGame.Event;
            Site = pgnGame.Site;
            Date = pgnGame.Date.ToString();
            Round = pgnGame.Round.ToString();
            White = pgnGame.White;
            Black = pgnGame.Black;
            Result = pgnGame.Result.ToString();
            Moves = board.Moves.Select(y => new PgnMove(y.From.ToString(),y.To.ToString(), y.PgnText));
        }

        public string Event { get; }
        public string Site { get; }
        public string Date { get; }
        public string Round { get; }
        public string White { get; }
        public string Black { get; }
        public string Result { get; }

        private readonly List<PgnMove> _pgnMoves = new List<PgnMove>();
        public IEnumerable<PgnMove> Moves { get; }
    }
    

    public class PgnMove
    {
        public string From { get; }
        public string To { get; }
        public string PgnText { get; }

        public PgnMove(string @from, string to, string pgnText)
        {
            To = to;
            PgnText = pgnText;
            From = @from;
        }

        public override string ToString()
        {
            return $"{To.ToLower()}-{From.ToLower()}";
        }
    }
}
