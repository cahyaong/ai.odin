// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenarioBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 3:49:46 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public record ScenarioBlueprint
{
    public static ScenarioBlueprint None { get; } = new()
    {
        Id = DefinedText.None,
        UniverseBlueprint = UniverseBlueprint.None,
        EntityPopulationBlueprints = []
    };

    public required string Id { get; init; }

    public required UniverseBlueprint UniverseBlueprint { get; init; }

    public required IEnumerable<EntityPopulationBlueprint> EntityPopulationBlueprints { get; init; }
}