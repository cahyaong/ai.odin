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
using nGratis.AI.Odin.Engine;

public partial class ComponentFactory : Node, IComponentFactory
{
    private readonly PackedScene _renderableEntity;

    private Node _entityPool;

    public ComponentFactory()
    {
        this._renderableEntity = (PackedScene)ResourceLoader.Load("res://ECS.Entity/RenderableEntity.tscn");
    }

    public override void _Ready()
    {
        this._entityPool = this
            .GetParent()
            .GetNode<Node>("EntityPool");
    }

    public IComponent CreateComponent(ComponentBlueprint blueprint)
    {
        var isRenderingBlueprint = blueprint.Id.Equals("Rendering", StringComparison.OrdinalIgnoreCase);

        if (!isRenderingBlueprint)
        {
            return Component.Unknown;
        }

        var renderableEntity = (RenderableEntity)this._renderableEntity.Instantiate();
        this._entityPool.AddChild(renderableEntity);

        return new RenderingComponent
        {
            RenderableEntity = renderableEntity
        };
    }
}