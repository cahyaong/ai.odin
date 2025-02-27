// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFactory.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 6:59:38 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

public partial class EntityFactory : Node, IEntityFactory
{
    private readonly PackedScene _entityScene;

    private Node _entityPoolNode;

    private uint _entityTotalCount;

    public EntityFactory()
    {
        this._entityScene = (PackedScene)ResourceLoader.Load("res://ECS.Entity/Entity.tscn");
        this._entityTotalCount = 0;
    }

    public override void _Ready()
    {
        this._entityPoolNode = this
            .GetParent()
            .GetNode("EntityPool");
    }

    public IUniverse CreateUniverse(float width, float height)
    {
        Guard
            .Require(width, nameof(width))
            .Is.GreaterThan(0.0f);

        Guard
            .Require(height, nameof(height))
            .Is.GreaterThan(0.0f);

        return new Universe
        {
            Width = width,
            Height = height
        };
    }

    public IEntity CreateEntity()
    {
        var entity = new Entity
        {
            Id = $"[_ENTITY_{this._entityTotalCount++:D4}_]"
        };

        var entityNode = (Node2D)this._entityScene.Instantiate();

        entity.AddComponent(
            new PositionComponent(),
            new RenderingComponent
            {
                EntityNode = entityNode
            });

        this._entityPoolNode.AddChild(entityNode);

        return entity;
    }
}