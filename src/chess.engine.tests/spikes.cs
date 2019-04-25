using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using chess.engine.Pieces;
using NUnit.Framework;

namespace chess.engine.tests
{
    [TestFixture]
    public class spikes
    {
        [Test]
        public void Should()
        {
            var engine = new BoardGameEngine<ChessPieceEntity>();
            engine.InitBoard();

            var startLocation = BoardLocation.At("B2");
            var pawnEntity = ChessPieceEntityFactory.Create(ChessPieceName.Pawn, Colours.White);
            engine.AddEntity(pawnEntity, startLocation);

            var piece = engine.PieceAt("B2");

            Assert.That(piece.Entity.EntityType, Is.EqualTo(ChessPieceName.Pawn));

            var paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(3));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(4));
            
            //            Assert.That(boardPiece.Paths.Count(), Is.EqualTo(2));
            //
            //            Assert.That(engine.PieceAt("A3").Pressure.White.Count, Is.EqualTo(1));
            //            Assert.That(engine.PieceAt("B3").Pressure.White.Count, Is.EqualTo(0));
            //            Assert.That(engine.PieceAt("C3").Pressure.White.Count, Is.EqualTo(1));
        }
    }

    #region Specific to the game of chess

    public static class ChessPieceEntityFactory
    {
        private static IDictionary<ChessPieceName, Func<Colours, ChessPieceEntity>> _factory = new Dictionary<ChessPieceName, Func<Colours, ChessPieceEntity>> 
        {
            {
                ChessPieceName.Pawn, (c) => new PawnEntity(c)
            }
        };
        public static ChessPieceEntity Create(ChessPieceName chessPiece, Colours player)
        {
            return _factory[chessPiece](player);
        }
    }


    public abstract class ChessPieceEntity : IBoardEntityType<ChessPieceName>
    {
        protected ChessPieceEntity(ChessPieceName piece, Colours owner) : base()
        {
            EntityType = piece;
            Player = owner;
        }
        public ChessPieceName EntityType { get; }

        public Colours Player { get; }
        public abstract IEnumerable<IMoveGenerator> MoveGenerators { get; }
    }

    public class PawnEntity : ChessPieceEntity
    {
        public PawnEntity(Colours owner) : base(ChessPieceName.Pawn, owner)
        {
        }
        public override IEnumerable<IMoveGenerator> MoveGenerators =>
            new List<IMoveGenerator>
            {
                new PawnRightTakeMoveGenerator(), new PawnNormalAndStartingMoveGenerator(), new PawnLeftTakeMoveGenerator()
            };

    }
    #endregion

    #region Specific to games played on a chess board    

    public interface IBoardEntity
    {
        Colours Player { get; } // TODO: Shoulld be a proper abstraction instead of using Colurs
        IEnumerable<IMoveGenerator> MoveGenerators { get; }
    }
    public interface IBoardEntityType<T> : IBoardEntity
    {
        T EntityType { get; }
    }
    public class BoardPiece<T> 
    {
        public T Entity { get; }
        public IEnumerable<Path> Paths { get; }
        public BoardPiece(T entityAt, IEnumerable<Path> paths)
        {
            Entity = entityAt;
            Paths = paths;
        }
    }

    public class BoardGameEngine<T> where T : IBoardEntity
    {
        private readonly Dictionary<BoardLocation, T> _entities = new Dictionary<BoardLocation, T>();
        private readonly Dictionary<BoardLocation, IEnumerable<Path>> _moves = new Dictionary<BoardLocation, IEnumerable<Path>>();

        public void InitBoard()
        {
        }

        public void AddEntity(T create, BoardLocation startingLocation)
        {
            _entities.Add(startingLocation, create);
            _moves.Add(startingLocation, null);
        }

        public BoardPiece<T> PieceAt(BoardLocation location)
        {
            var entityAt = _entities[location];

            // Get Move Generators for entity type & generate paths
            var moves = MovesForEntityAt(entityAt, location);

            // ? What do we do with the paths?

            // ? Do we want to do this every time we get an entity?
            // ? When do we refresh the move list ?

            return new BoardPiece<T>(entityAt, moves);
        }
        public BoardPiece<T> PieceAt(string b2)
        {
            return PieceAt((BoardLocation)b2);

        }

        private IEnumerable<Path> MovesForEntityAt(T entityAt, BoardLocation boardLocation)
        {
            if (_moves.TryGetValue(boardLocation, out IEnumerable<Path> moves))
            {
                if (moves == null)
                {
                    moves = GenerateMovesFrom(boardLocation, entityAt);
                    _moves[boardLocation] = moves;
                }

                return moves;
            }

            return new List<Path>();
        }

        private IEnumerable<Path> GenerateMovesFrom(BoardLocation boardLocation, T entity)
        {
            var paths = new List<Path>();

            foreach (var moveGen in entity.MoveGenerators)
            {
                paths.AddRange(moveGen.MovesFrom(boardLocation, entity.Player));
            }

            return paths;
        }
    }


    #endregion


}