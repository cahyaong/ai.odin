// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Size.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, March 30, 2025 1:51:30 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace nGratis.AI.Odin.Engine;

[DebuggerDisplay("<Size> ({this.Width}, {this.Height})")]
public record Size
{
    public static Size Unknown { get; } = new()
    {
        Width = float.NaN,
        Height = float.NaN
    };

    public static Size Zero { get; } = new()
    {
        Width = 0.0f,
        Height = 0.0f
    };

    public required float Width { get; init; }

    public required float Height { get; init; }
}