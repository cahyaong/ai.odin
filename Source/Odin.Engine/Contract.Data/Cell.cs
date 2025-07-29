// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cell.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 29, 2025 1:55:41 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Diagnostics;

[DebuggerDisplay("<Cell ({this.Row}, {this.Column})>")]
public record Cell
{
    public required int Row { get; init; }

    public required int Column { get; init; }
};