using System;
using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IBoardEntity: ICloneable
    {
        IEnumerable<object> PathGenerators { get; }

        Colours Owner { get; }

        object EntityType { get; }

    }
    public interface IBoardEntity<out TEntityType> : IBoardEntity
    {
        new IEnumerable<IPathGenerator> PathGenerators { get; }

        new TEntityType EntityType { get; }
    }
}