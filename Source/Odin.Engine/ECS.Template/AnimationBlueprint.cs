// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, July 26, 2025 5:08:23 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record AnimationBlueprint
{
    public required string Id { get; init; }

    public required Cell StartingCell { get; init; }

    public required Cell EndingCell { get; init; }
}