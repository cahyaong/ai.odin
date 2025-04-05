// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingComponent.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:19:56 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;

public record RenderingComponent : IComponent
{
    public required RenderableEntity RenderableEntity { get; init; }
}