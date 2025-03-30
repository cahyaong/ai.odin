// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhysicsComponent.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, March 11, 2025 6:01:02 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record PhysicsComponent : IComponent
{
    public PhysicsComponent()
    {
        this.Position = Point.Unknown;
        this.Velocity = Vector.Unknown;
    }

    public Point Position { get; set; }

    public Vector Velocity { get; set; }
}