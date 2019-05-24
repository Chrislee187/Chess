using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace chess.pgn.Json
{
    public class PgnJson : Dictionary<string, string>
    {
        public string Moves { get; set; }

        public PgnJson(PgnGame game)
        {
            game.TagPairs.ToList().ForEach(tp => Add(tp.Key, tp.Value));

            Add("Moves", game.MoveText);

            Moves = game.MoveText;
        }
    }
}