// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlacementBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 3:56:48 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record PlacementBlueprint
{
    public static PlacementBlueprint None { get; } = new()
    {
        PlacementStrategy = PlacementStrategy.Unknown,
        CenterPoint = Point.Zero,
        RegionSize = Size.Zero
    };

    public required PlacementStrategy PlacementStrategy { get; init; }

    public required Point CenterPoint { get; init; }

    public required Size RegionSize { get; init; }
}

public enum PlacementStrategy
{
    Unknown = 0,
    Random
}