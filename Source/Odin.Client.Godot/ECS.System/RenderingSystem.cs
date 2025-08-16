// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingSystem.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:24:23 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using System.Collections.Generic;
using nGratis.AI.Odin.Engine;

[SystemMetadata(OrderingIndex = int.MaxValue)]
public partial class RenderingSystem : BaseFixedSystem
{
    public RenderingSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(PhysicsComponent),
        typeof(RenderingComponent)
    ];

    protected override void ProcessEntity(uint _, IGameState __, IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        var renderingComponent = entity.FindComponent<RenderingComponent>();

        var scaledCoordinate = physicsComponent.Position * Constant.PixelPerUnit;
        renderingComponent.RenderableEntity.Position = scaledCoordinate.ToVector2();

        renderingComponent.RenderableEntity.UpdateAnimationState(vitalityComponent.EntityState);
    }
}