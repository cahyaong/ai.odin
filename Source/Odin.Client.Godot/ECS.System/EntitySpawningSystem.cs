// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntitySpawningSystem.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:06:06 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using System.Collections.Generic;
using System.Linq;
using nGratis.AI.Odin.Engine;

[SystemMetadata(OrderingIndex = int.MinValue + 128)]
public class EntitySpawningSystem : BaseSystem
{
    private readonly IEntityFactory _entityFactory;

    private readonly Random _random;

    public EntitySpawningSystem(
        ScenarioBlueprint scenarioBlueprint,
        IEntityFactory entityFactory,
        IEntityManager entityManager)
        : base(scenarioBlueprint, entityManager)
    {
        this._entityFactory = entityFactory;
        this._random = new Random();
    }

    public override void ProcessFixedDuration(uint tick, IGameState gameState)
    {
        Enumerable
            .Range(0, this._random.Next(1, 5))
            .Select(_ => this.CreateEntity(gameState.Universe, "Human"))
            .ForEach(this.EntityManager.AddEntity);

        Enumerable
            .Range(0, this._random.Next(1, 3))
            .Select(_ => this.CreateEntity(gameState.Universe, "BlueberryBush"))
            .ForEach(this.EntityManager.AddEntity);
    }

    private IEntity CreateEntity(IUniverse universe, string blueprintId)
    {
        var entity = this._entityFactory.CreateEntity(blueprintId);
        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        physicsComponent.Position = new Point
        {
            X = this._random.NextSingle() * universe.Width,
            Y = this._random.NextSingle() * universe.Height
        };

        return entity;
    }
}