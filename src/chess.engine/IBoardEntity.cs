using System;
using System.Collections.Generic;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IBoardEntity<out TEntityType>: ICloneable
    {
        IEnumerable<IPathGenerator> PathGenerators { get; }
        TEntityType EntityType { get; }
    }
}