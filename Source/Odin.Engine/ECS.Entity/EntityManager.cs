﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityManager.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 6:33:57 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections.Immutable;

public class EntityManager : IEntityManager
{
    private readonly IDictionary<Type, IList<IEntity>> _entitiesByComponentTypeLookup;

    public EntityManager()
    {
        this._entitiesByComponentTypeLookup = new Dictionary<Type, IList<IEntity>>();
    }

    public static IEntityManager Unknown => UnknownEntityManager.Instance;

    public uint TotalCount { get; private set; }

    public void AddEntity(IEntity entity)
    {
        entity
            .FindComponents()
            .ForEach(component =>
            {
                if (this._entitiesByComponentTypeLookup.TryGetValue(component.GetType(), out var entities))
                {
                    entities.Add(entity);
                }
                else
                {
                    this._entitiesByComponentTypeLookup.Add(component.GetType(), new List<IEntity> { entity });
                }
            });

        this.TotalCount++;
    }

    public IReadOnlyCollection<IEntity> FindEntities(params IReadOnlyCollection<Type> componentTypes)
    {
        // TODO (SHOULD): Optimize filtering by starting from component type with the least entities!

        if (!this._entitiesByComponentTypeLookup.TryGetValue(componentTypes.First(), out var entities))
        {
            return [];
        }

        var remainingComponentTypes = componentTypes
            .Skip(1)
            .ToArray();

        return entities
            .Where(entity => entity.HasComponent(remainingComponentTypes))
            .ToImmutableArray();
    }
}