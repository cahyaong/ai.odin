// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Boundary.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, September 7, 2025 2:52:16 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record Boundary
{
    public required Point StartingPoint { get; init; }

    public required Point EndingPoint { get; init; }
}