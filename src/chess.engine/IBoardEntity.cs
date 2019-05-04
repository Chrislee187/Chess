using System;
using System.Collections.Generic;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IBoardEntity<out TEntityType, out TOwner> : ICloneable
    {
        IEnumerable<IPathGenerator> PathGenerators { get; }

        TOwner Owner { get; }

        TEntityType EntityType { get; }
    }
}