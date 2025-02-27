// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionComponent.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:22:55 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record PositionComponent : IComponent
{
    public float X { get; set; }

    public float Y { get; set; }
}