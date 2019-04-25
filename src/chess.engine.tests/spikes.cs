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
            var engine = new ChessGameEngine();
            engine.InitBoard();

            var startLocation = BoardLocation.At("B2");
            var whitePawn = ChessPieceEntityFactory.Create(ChessPieceName.Pawn, Colours.White);
            var blackPawn = ChessPieceEntityFactory.Create(ChessPieceName.Pawn, Colours.Black);
            engine
                .AddEntity(whitePawn, startLocation)
                .AddEntity(blackPawn, BoardLocation.At("D5"))
                .AddEntity(whitePawn, BoardLocation.At("C5"));

            var piece = engine.PieceAt("B2");

            Assert.That(piece.Entity.EntityType, Is.EqualTo(ChessPieceName.Pawn));

            var paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(1));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(2));

            piece = engine.PieceAt("C5");
            Assert.That(piece.Entity.EntityType, Is.EqualTo(ChessPieceName.Pawn));
            paths = piece.Paths;
            Assert.That(paths.Count(), Is.EqualTo(2));
            Assert.That(paths.SelectMany(m => m).Count(), Is.EqualTo(2));

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

    public abstract class ChessPieceEntity 
    {
        protected ChessPieceEntity(ChessPieceName piece, Colours owner) 
        {
            EntityType = piece;
            Player = owner;
        }
        public ChessPieceName EntityType { get; }

        public Colours Player { get; }
        public abstract IEnumerable<IPathGenerator> PathGenerators { get; }
    }

    public class PawnEntity : ChessPieceEntity
    {
        public PawnEntity(Colours owner) : base(ChessPieceName.Pawn, owner)
        {
        }
        public override IEnumerable<IPathGenerator> PathGenerators =>
            new List<IPathGenerator>
            {
                new PawnRightTakePathGenerator(), new PawnNormalAndStartingPathGenerator(), new PawnLeftTakePathGenerator()
            };

    }

    public class ChessMoveValidator : IMoveValidator<ChessMove>
    {
        public Path ValidPath(Path possiblePath, BoardPieceGetter pieceGetter)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                var tests = new List<Func<ChessMove, BoardPieceGetter, bool>>();
            
                switch (move.ChessMoveType)
                {
                    case ChessMoveType.MoveOnly:
                        tests.Add(DestinationIsEmpty);
                        break;
                    case ChessMoveType.TakeOnly:
                        tests.Add(DestinationContainsEnemy);
                        break;
                        case ChessMoveType.TakeEnPassant:
            
                            tests.Add(EnPassantIsPossible);
                            break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(move.ChessMoveType), move.ChessMoveType, $"NotImplemented ChessMoveType");
                }

                if (!tests.All(t => t(move, pieceGetter)))
                {
                    break;
                }

                validPath.Add(move);
            }

            return validPath;
        }

        private bool EnPassantIsPossible(ChessMove move, BoardPieceGetter safeGetBoardPiece)
        {
            var normalTakeOk = DestinationContainsEnemy(move, safeGetBoardPiece);
        
            var piece = safeGetBoardPiece(move.From);
        
            var passingPieceLocation = move.To.MoveBack(piece.Player);
            var passingPiece = safeGetBoardPiece(passingPieceLocation);

            if (passingPiece == null) return false;
            if (passingPiece.Player == piece.Player) return false;
            if (passingPiece.EntityType != ChessPieceName.Pawn) return false;

            var enpassantOk = CheckPawnUsedDoubleMove(move.To);
        
        
            return normalTakeOk || enpassantOk;
        }

        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            return true;
        }

        private bool DestinationContainsEnemy(ChessMove move, BoardPieceGetter safeGetBoardPiece)
        {
            var sourcePiece = safeGetBoardPiece(move.From);
            Guard.NotNull(sourcePiece);

            var destinationPiece = safeGetBoardPiece(move.To);
            if (destinationPiece == null) return false;
        
            return sourcePiece.Player != destinationPiece.Player;
        }
        private bool DestinationIsEmpty(ChessMove move, BoardPieceGetter getter) => LocationIsEmpty(move.To, getter);
        private bool LocationIsEmpty(BoardLocation location, BoardPieceGetter getter) => getter(location) == null;

    }
    #endregion

    #region Specific to games played on a chess board    

    public interface IBoardEntity
    {
        Colours Player { get; } // TODO: Shoulld be a proper abstraction instead of using Colurs
    }
    public interface ITypedBoardEntity<TEntity, TMove> : IBoardEntity
    {
        TEntity EntityType { get; }
        IEnumerable<IPathGenerator> PathGenerators { get; }
    }
    public class BoardPiece
    {
        public ChessPieceEntity Entity { get; }
        public IEnumerable<Path> Paths { get; }
        public BoardPiece(ChessPieceEntity entityAt, IEnumerable<Path> paths)
        {
            Entity = entityAt;
            Paths = paths;
        }
    }

    public class ChessGameEngine
    {
        private readonly Dictionary<BoardLocation, ChessPieceEntity> _entities = new Dictionary<BoardLocation, ChessPieceEntity>();
        private readonly Dictionary<BoardLocation, IEnumerable<Path>> _paths = new Dictionary<BoardLocation, IEnumerable<Path>>();

        public void InitBoard()
        {
        }

        public ChessGameEngine AddEntity(ChessPieceEntity create, BoardLocation startingLocation)
        {
            _entities.Add(startingLocation, create);
            _paths.Add(startingLocation, null);

            return this;
        }

        public BoardPiece PieceAt(BoardLocation location)
        {
            var boardPiece = SafeGetEntity(location);
            if (boardPiece == null) return null;

            var validPaths = ValidMovesForEntityAt(boardPiece, location);

            return new BoardPiece(boardPiece, validPaths);
        }

        public BoardPiece PieceAt(string location) => PieceAt((BoardLocation)location);

        private IEnumerable<Path> ValidMovesForEntityAt(ChessPieceEntity entityAt, BoardLocation boardLocation)
        {
            if (_paths.TryGetValue(boardLocation, out IEnumerable<Path> possiblePaths))
            {
                if (possiblePaths == null)
                {
                    possiblePaths = GeneratePossibleMoves(boardLocation, entityAt).ToList();

                    var validPaths = RemoveInvalidMoves(possiblePaths).ToList();
                    _paths[boardLocation] = validPaths;
                    return validPaths;
                }

                return possiblePaths;
            }

            return new List<Path>();
        }

        // MoveTypes are specific to the game of chess and therefore should NOT be in the BoardGameEnginge
        private IEnumerable<Path> RemoveInvalidMoves(IEnumerable<Path> possiblePaths)
        {
            var validator = new ChessMoveValidator() ;

            var validPaths = new List<Path>();

            foreach (var possiblePath in possiblePaths)
            {
                var validPath = validator.ValidPath(possiblePath, SafeGetEntity);

                if (validPath.Any())
                {
                    validPaths.Add(validPath);
                }
            }

            return validPaths;
        }

        private IEnumerable<Path> GeneratePossibleMoves(BoardLocation boardLocation, ChessPieceEntity entity)
        {
            var paths = new List<Path>();

            foreach (var pathGen in entity.PathGenerators)
            {
                var movesFrom = pathGen.PathsFrom(boardLocation, entity.Player);
                paths.AddRange(movesFrom);
            }

            return paths;
        }

        private ChessPieceEntity SafeGetEntity(BoardLocation location)
        {
            _entities.TryGetValue(location, out var entityAt);

            return entityAt;
        }
    }
    public delegate ChessPieceEntity BoardPieceGetter(BoardLocation location);
    internal interface IMoveValidator<T>
    {
        Path ValidPath(Path possiblePath, BoardPieceGetter pieceGetter);
    }

    #endregion


}