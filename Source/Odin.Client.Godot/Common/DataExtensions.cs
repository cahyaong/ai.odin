// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataExtensions.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, March 11, 2025 6:23:17 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace nGratis.AI.Odin.Engine;

public static class DataExtensions
{
    public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);
}