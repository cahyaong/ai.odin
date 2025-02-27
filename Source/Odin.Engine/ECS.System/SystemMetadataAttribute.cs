// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemMetadataAttribute.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Thursday, February 27, 2025 2:48:04 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

[AttributeUsage(AttributeTargets.Class)]
public class SystemMetadataAttribute : Attribute
{
    public int OrderingIndex { get; init; }
}