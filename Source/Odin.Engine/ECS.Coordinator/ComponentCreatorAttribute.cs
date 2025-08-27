// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentCreatorAttribute.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, August 23, 2025 5:52:33 PM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Method)]
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
public sealed class ComponentCreatorAttribute : Attribute
{
    public required Type CreatedType { get; init; }
}