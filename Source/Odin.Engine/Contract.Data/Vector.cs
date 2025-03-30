// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, March 11, 2025 6:08:01 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace nGratis.AI.Odin.Engine;

[DebuggerDisplay("<Vector> ({this.X}, {this.Y})")]
public record Vector
{
    public static Vector Unknown { get; } = new()
    {
        X = float.NaN,
        Y = float.NaN
    };

    public static Vector Zero { get; } = new()
    {
        X = 0.0f,
        Y = 0.0f
    };

    public required float X { get; init; }

    public required float Y { get; init; }

    public static Vector operator *(Vector leftVector, Vector rightVector)
    {
        return new Vector
        {
            X = leftVector.X * rightVector.X,
            Y = leftVector.Y * rightVector.Y
        };
    }

    public Vector Sign()
    {
        return new Vector
        {
            X = MathF.Sign(this.X),
            Y = MathF.Sign(this.Y)
        };
    }
}