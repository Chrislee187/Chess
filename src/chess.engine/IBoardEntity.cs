using System;
using System.Collections.Generic;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IBoardEntity: ICloneable
    {
        IEnumerable<IPathGenerator> PathGenerators { get; }

        string EntityName { get; }

        int EntityType { get; }

        int Owner { get; }
    }
}