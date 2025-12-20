// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 4:40:44 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record AnimationBlueprint
{
    public required string Name { get; init; }

    public required Cell StartingCell { get; init; }

    public required Cell EndingCell { get; init; }
}