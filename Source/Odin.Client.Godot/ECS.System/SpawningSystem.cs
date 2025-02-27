// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpawningSystem.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:06:06 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using nGratis.AI.Odin.Engine;

public class SpawningSystem : ISystem
{
    private readonly IEntityFactory _entityFactory;

    private readonly IEntityManager _entityManager;

    private readonly Random _random;

    public SpawningSystem(IEntityFactory entityFactory, IEntityManager entityManager)
    {
        this._entityFactory = entityFactory;
        this._entityManager = entityManager;
        this._random = new Random();
    }

    public void Process(uint tick, IGameState gameState)
    {
        var entity = this._entityFactory.CreateEntity();
        var positionComponent = entity.FindComponent<PositionComponent>();

        positionComponent.X = this._random.NextSingle() * gameState.Universe.Width;
        positionComponent.Y = this._random.NextSingle() * gameState.Universe.Height;

        this._entityManager.AddEntity(entity);
    }
}