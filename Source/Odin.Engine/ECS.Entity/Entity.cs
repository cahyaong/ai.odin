// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Entity.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:15:23 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections.Immutable;
using nGratis.Cop.Olympus.Contract;

public record Entity : IEntity
{
    private readonly IDictionary<Type, IComponent> _componentByComponentTypeLookup;

    public Entity()
    {
        this._componentByComponentTypeLookup = new Dictionary<Type, IComponent>();
    }

    public required string Id { get; init; }

    public void AddComponent(params IComponent[] components)
    {
        var componentByComponentTypeLookup = components.ToImmutableDictionary(component => component.GetType());

        var existingTypes = this
            ._componentByComponentTypeLookup.Keys
            .Intersect(componentByComponentTypeLookup.Keys)
            .ToImmutableArray();

        if (existingTypes.Any())
        {
            throw new OdinException(
                "Component with same type must be defined once!",
                ("Existing Type(s)", $"({existingTypes.ToPrettifiedText(type => type.Name)})"));
        }

        componentByComponentTypeLookup.ForEach(pair => this._componentByComponentTypeLookup.Add(pair.Key, pair.Value));
    }

    public void RemoveComponents()
    {
        this._componentByComponentTypeLookup.Clear();
    }

    public bool HasComponent<TComponent>()
        where TComponent : IComponent
    {
        return this._componentByComponentTypeLookup.ContainsKey(typeof(TComponent));
    }

    public bool HasComponent(params Type[] componentTypes)
    {
        return this._componentByComponentTypeLookup
            .Keys
            .Intersect(componentTypes)
            .Count() == componentTypes.Length;
    }

    public IEnumerable<IComponent> FindComponents()
    {
        return this._componentByComponentTypeLookup.Values;
    }

    public TComponent FindComponent<TComponent>()
        where TComponent : IComponent
    {
        if (!this._componentByComponentTypeLookup.TryGetValue(typeof(TComponent), out var component))
        {
            throw new OdinException(
                "Entity does not have target component!",
                ("ID", this.Id),
                ("Component Type", typeof(TComponent).FullName ?? DefinedText.Unknown));
        }

        return (TComponent)component;
    }
}