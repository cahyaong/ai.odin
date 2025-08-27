// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:14:24 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Diagnostics.CodeAnalysis;

public interface IEntity
{
    string Id { get; }

    void AddComponent(params IComponent[] components);

    void RemoveComponents();

    bool HasComponent<TComponent>() where TComponent : IComponent;

    bool HasComponent(params Type[] componentTypes);

    IEnumerable<IComponent> FindComponents();

    TComponent FindComponent<TComponent>() where TComponent : IComponent;

    bool TryFindComponent<TComponent>([MaybeNullWhen(false)] out TComponent component) where TComponent : IComponent;
}