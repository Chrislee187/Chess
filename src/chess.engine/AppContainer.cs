using System;
using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace chess.engine
{
    public static class AppContainer
    {
        private static ServiceProvider ServiceProvider { get; }

        static AppContainer()
        {
            var serviceCollection = new ServiceCollection();
            var config = ConfigureConfig(serviceCollection);
            ConfigureLogging(serviceCollection, config);
            AddChessDependencies(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static T GetService<T>()
            => ServiceProvider.GetService<T>();

        private static IConfigurationRoot ConfigureConfig(this IServiceCollection serviceCollection)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddSingleton(config);
            return config;
        }

        private static void ConfigureLogging(this IServiceCollection serviceCollection, IConfigurationRoot config)
        {
            // Add logging
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .CreateLogger();

        }

        public static void AddChessDependencies(this IServiceCollection services)
        {
            services.AddTransient<IRefreshAllPaths<ChessPieceEntity>, ChessRefreshAllPaths>();
            services.AddTransient<IPathsValidator<ChessPieceEntity>, ChessPathsValidator>();
            services.AddTransient<IPathValidator<ChessPieceEntity>,ChessPathValidator>();
            services.AddTransient<IMoveValidationFactory<ChessPieceEntity>, ChessMoveValidationProvider>();

            services.AddTransient<IBoardEngineProvider<ChessPieceEntity>,ChessBoardEngineProvider>();
            services.AddTransient<IBoardActionFactory<ChessPieceEntity>,ChessBoardActionProvider>();
            services.AddTransient<IBoardEntityFactory<ChessPieceEntity>,ChessPieceEntityFactory>();
            services.AddTransient<IChessGameStateService,ChessGameStateService>();

        }
    }
}