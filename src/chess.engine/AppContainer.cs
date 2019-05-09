﻿using System;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;
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
            var config = serviceCollection.ConfigureConfig();
            serviceCollection.ConfigureLogging(config);
            serviceCollection.AddChessDependencies();

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
            services.AddTransient<IRefreshAllPaths<ChessPieceEntity>, 
                ChessRefreshAllPaths>();
            services.AddTransient<IMoveValidationFactory<ChessPieceEntity>,
                MoveValidationFactory<ChessPieceEntity>>();

            services.AddTransient<IPathValidator<ChessPieceEntity>,
                ChessPathValidator>();
            services.AddTransient<IPathsValidator<ChessPieceEntity>,
                ChessPathsValidator>();

            services.AddTransient<IBoardEngineProvider<ChessPieceEntity>,ChessBoardEngineProvider>();
            services.AddTransient<IChessGameState,ChessGameState>();

        }
    }
}