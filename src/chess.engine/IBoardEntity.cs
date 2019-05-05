using System;
using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IBoardEntity: ICloneable
    {
        IEnumerable<IPathGenerator> PathGenerators { get; }

        Colours Owner { get; }

        string EntityName { get; }
    }
}