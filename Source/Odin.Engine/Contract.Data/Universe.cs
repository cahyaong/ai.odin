// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Universe.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 6:30:07 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record Universe : IUniverse
{
    public required float Width { get; init; }

    public required float Height { get; init; }
}