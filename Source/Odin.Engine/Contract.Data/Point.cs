// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Point.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, March 11, 2025 6:07:18 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Diagnostics;

[DebuggerDisplay("<Point> ({this.X}, {this.Y})")]
public record Point
{
    public static Point Unknown { get; } = new()
    {
        X = float.NaN,
        Y = float.NaN
    };

    public static Point Zero { get; } = new()
    {
        X = 0.0f,
        Y = 0.0f
    };

    public required float X { get; init; }

    public required float Y { get; init; }

    public static Point operator *(Point point, float scalar)
    {
        return new Point
        {
            X = point.X * scalar,
            Y = point.Y * scalar
        };
    }

    public static Point operator +(Point point, Vector vector)
    {
        return new Point
        {
            X = point.X + vector.X,
            Y = point.Y + vector.Y
        };
    }

    public static Vector operator -(Point leftPoint, Point rightPoint)
    {
        return new Vector
        {
            X = leftPoint.X - rightPoint.X,
            Y = leftPoint.Y - rightPoint.Y
        };
    }

    public bool IsCloseTo(Point point)
    {
        return
            MathF.Abs(this.X - point.X) <= float.Epsilon &&
            MathF.Abs(this.Y - point.Y) <= float.Epsilon;
    }

    public Point Clamp(Size size)
    {
        return new Point
        {
            X = MathF.Max(0.0f, MathF.Min(this.X, size.Width)),
            Y = MathF.Max(0.0f, MathF.Min(this.Y, size.Height))
        };
    }
}