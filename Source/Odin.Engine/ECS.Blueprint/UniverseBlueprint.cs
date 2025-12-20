// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniverseBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 4:43:01 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record UniverseBlueprint
{
    public static UniverseBlueprint None { get; } = new()
    {
        Size = Size.Zero
    };

    public required Size Size { get; init; }
}