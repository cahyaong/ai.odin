// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFactory.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 6:59:38 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using nGratis.Cop.Olympus.Contract;

public class EntityFactory : IEntityFactory
{
    private readonly IReadOnlyDictionary<string, EntityBlueprint> _entityBlueprintByIdLookup;

    private readonly IReadOnlyList<IComponentFactory> _componentFactories;

    private uint _totalCount;

    public EntityFactory(IDataStore dataStore, IEnumerable<IComponentFactory> componentFactories)
    {
        this._entityBlueprintByIdLookup = dataStore
            .LoadEntityBlueprints()
            .ToImmutableDictionary(blueprint => blueprint.Id);

        this._componentFactories = componentFactories.ToImmutableArray();
        this._totalCount = 0;
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
            Size = new Size
            {
                Width = width,
                Height = height
            }
        };
    }

    public IEntity CreateEntity(string blueprintId)
    {
        var entity = new Entity
        {
            Id = $"ENTITY-{blueprintId}-{this._totalCount++:D4}"
        };

        // TODO (SHOULD): Add default rendering component or add tracking when blueprint is not found for given ID!

        if (!this._entityBlueprintByIdLookup.TryGetValue(blueprintId, out var entityBlueprint))
        {
            return entity;
        }

        foreach (var componentBlueprint in entityBlueprint.ComponentBlueprints)
        {
            var component = this
                ._componentFactories
                .Select(componentFactory => componentFactory.CreateComponent(componentBlueprint))
                .FirstOrDefault(component => component != Component.Unknown);

            if (component != null)
            {
                entity.AddComponent(component);
            }
        }

        return entity;
    }
}