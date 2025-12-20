// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityPopulationBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 4:32:13 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record EntityPopulationBlueprint
{
    public required string EntityId { get; init; }

    public required int Quantity { get; init; }

    public required PlacementBlueprint PlacementBlueprint { get; init; }

    public required SpawningBlueprint SpawningBlueprint { get; init; }
}