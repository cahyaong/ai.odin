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
using System.Reflection;

public class GameController : IGameController
{
    private readonly IGameState _gameState;

    private readonly ITimeTracker _timeTracker;

    private readonly IReadOnlyCollection<ISystem> _systems;

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
        this._timeTracker.DeltaChanged += this.OnDeltaChanged;
        this._timeTracker.TickChanged += this.OnTickChanged;

        this._systems = systems
            .OrderBy(system => system
                .GetType()
                .GetTypeInfo()
                .GetCustomAttribute<SystemMetadataAttribute>()?
                .OrderingIndex ?? 0)
            .ToImmutableArray();
    }

    public void Start()
    {
        this._timeTracker.Start();
    }

    public void End()
    {
        this._timeTracker.End();
    }

    private void OnDeltaChanged(object? _, DeltaChangedEventArgs args)
    {
        this._systems
            .ForEach(system => system.ProcessVariableDuration(args.Value, this._gameState));
    }

    private void OnTickChanged(object? _, TickChangedEventArgs args)
    {
        this._systems
            .ForEach(system => system.ProcessFixedDuration(args.Value, this._gameState));
    }
}