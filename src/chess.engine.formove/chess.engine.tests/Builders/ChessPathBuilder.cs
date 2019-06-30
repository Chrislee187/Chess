using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Extensions;

namespace chess.engine.tests.Builders
{
    public class ChessPathBuilder
    {
        public ChessPathDestinationsBuilder From(BoardLocation at) => new ChessPathDestinationsBuilder(at);

        public ChessPathDestinationsBuilder From(string at) => From((BoardLocation) at.ToBoardLocation());

        public Path Build()
        {
            return new ChessPathDestinationsBuilder("D2".ToBoardLocation())
                .To("D4".ToBoardLocation(), (int) DefaultActions.MoveOnly).Build();
        }
    }
}