// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingSystem.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:24:23 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Collections.Generic;
using nGratis.AI.Odin.Engine;

[SystemMetadata(OrderingIndex = int.MaxValue)]
public partial class RenderingSystem : BaseSystem
{
    private static readonly float PixelPerUnit = 16;

    private readonly IEntityManager _entityManager;

    public RenderingSystem(IEntityManager entityManager)
    {
        this._entityManager = entityManager;
    }

    public override void ProcessFixedDuration(uint _, IGameState __)
    {
        this._entityManager
            .FindEntities(typeof(PositionComponent), typeof(RenderingComponent))
            .ForEach(entity =>
            {
                var positionComponent = entity.FindComponent<PositionComponent>();
                var renderingComponent = entity.FindComponent<RenderingComponent>();

                renderingComponent.EntityNode.Position = new Vector2(
                    positionComponent.X * PixelPerUnit,
                    positionComponent.Y * PixelPerUnit);
            });
    }
}