// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameController.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:24:05 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

public class GameController : IGameController
{
    private readonly IGameState _gameState;

    private readonly ITimeTracker _timeTracker;

    private readonly IReadOnlyCollection<ISystem> _systems;

    private TimeSpan _maxVariableExecutionPeriod;

    public GameController(
        ITimeTracker timeTracker,
        IEntityFactory entityFactory,
        IReadOnlyCollection<ISystem> systems)
    {
        this._gameState = new GameState
        {
            Universe = entityFactory.CreateUniverse(64, 32)
        };

        this._timeTracker = timeTracker;

        this._systems = systems
            .OrderBy(system => system
                .GetType()
                .GetTypeInfo()
                .GetCustomAttribute<SystemMetadataAttribute>()?
                .OrderingIndex ?? 0)
            .ToImmutableArray();

        this._maxVariableExecutionPeriod = TimeSpan.Zero;
    }

    public void Start()
    {
        this._timeTracker.DeltaChanged += this.OnDeltaChanged;
        this._timeTracker.TickChanged += this.OnTickChanged;

        this._timeTracker.Start();
    }

    public void End()
    {
        this._timeTracker.End();

        this._timeTracker.DeltaChanged -= this.OnDeltaChanged;
        this._timeTracker.TickChanged -= this.OnTickChanged;
    }

    private void OnDeltaChanged(object? _, DeltaChangedEventArgs args)
    {
        var stopwatch = Stopwatch.StartNew();

        this._systems
            .ForEach(system => system.ProcessVariableDuration(args.Value, this._gameState));

        stopwatch.Stop();

        if (stopwatch.Elapsed > this._maxVariableExecutionPeriod)
        {
            this._maxVariableExecutionPeriod = stopwatch.Elapsed;
        }
    }

    private void OnTickChanged(object? _, TickChangedEventArgs args)
    {
        var stopwatch = Stopwatch.StartNew();

        this._systems
            .ForEach(system => system.ProcessFixedDuration(args.Value, this._gameState));

        stopwatch.Stop();

        var fixedExecutionPeriod = stopwatch.Elapsed;

        this._gameState.DebuggingStatistics
            .AddMetric("Variable Execution (ms)", this._maxVariableExecutionPeriod.TotalMilliseconds)
            .AddMetric("Fixed Execution (ms)", fixedExecutionPeriod.TotalMilliseconds);

        this._maxVariableExecutionPeriod = TimeSpan.Zero;
    }
}