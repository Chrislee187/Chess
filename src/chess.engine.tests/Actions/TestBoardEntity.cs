using System.Collections.Generic;
using board.engine;
using board.engine.Movement;

namespace chess.engine.tests.Actions
{
    public class TestBoardEntity : IBoardEntity
    {
        public object Clone()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IPathGenerator> PathGenerators { get; }
        public string EntityName { get; }
        public int EntityType { get; }
        public int Owner { get; }
    }
}