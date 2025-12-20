// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpawningBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 3:57:13 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record SpawningBlueprint
{
    public static SpawningBlueprint None { get; } = new()
    {
        SpawningStrategy = SpawningStrategy.Unknown
    };

    public required SpawningStrategy SpawningStrategy { get; init; }
}

public enum SpawningStrategy
{
    Unknown = 0,
    Initial
}