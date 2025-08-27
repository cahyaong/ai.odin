// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentFactory.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 22, 2025 5:23:52 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using nGratis.AI.Odin.Engine;

public partial class ComponentFactory : Node, IComponentFactory
{
    private readonly PackedScene _renderableEntityTemplate;

    private IReadOnlyDictionary<string, SpriteSheetBlueprint> _spriteSheetBlueprintByIdLookup;

    public ComponentFactory()
    {
        this._renderableEntityTemplate = (PackedScene)ResourceLoader.Load("res://ECS.Entity/RenderableEntity.tscn");
    }

    [Export]
    public EntityPool EntityPool { get; private set; }

    [Export]
    public SpriteSheetFactory SpriteSheetFactory { get; private set; }

    public override void _Ready()
    {
        this._spriteSheetBlueprintByIdLookup = this
            .GetParent<EntityCoordinator>()
            .DataStore
            .LoadSpriteSheetBlueprints()
            .ToImmutableDictionary(blueprint => blueprint.Id);
    }

    public IComponent CreateComponent(ComponentBlueprint componentBlueprint)
    {
        var isRenderingBlueprint = componentBlueprint.Id.Equals("Rendering", StringComparison.OrdinalIgnoreCase);

        if (!isRenderingBlueprint)
        {
            return Component.Unknown;
        }

        var renderingComponentBlueprint = new RenderingComponentBlueprint(componentBlueprint);

        var hasSpriteSheetBlueprint = this._spriteSheetBlueprintByIdLookup.TryGetValue(
            renderingComponentBlueprint.SpriteSheetBlueprintId,
            out var spritesheetBlueprint);

        if (!hasSpriteSheetBlueprint)
        {
            return Component.Unknown;
        }

        var spriteFrames = this.SpriteSheetFactory.CreateSpriteFrames(
            spritesheetBlueprint,
            renderingComponentBlueprint.TextureName);

        var renderableEntity = (RenderableEntity)this._renderableEntityTemplate.Instantiate();
        renderableEntity.UpdateSpritesheet(spriteFrames);

        this.EntityPool.AddChild(renderableEntity);

        return new RenderingComponent
        {
            RenderableEntity = renderableEntity
        };
    }
}