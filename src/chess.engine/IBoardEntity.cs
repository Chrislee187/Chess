using System;
using System.Collections.Generic;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IBoardEntity<out TEntityType/*, out TOwner*/> : ICloneable
    {
        IEnumerable<IPathGenerator> PathGenerators { get; }

        Colours Owner { get; }

        TEntityType EntityType { get; }
    }
}