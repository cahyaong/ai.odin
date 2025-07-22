// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, April 8, 2025 6:31:27 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record EntityBlueprint
{
    public required string Id { get; init; }

    public required IEnumerable<ComponentBlueprint> ComponentBlueprints { get; init; }
}