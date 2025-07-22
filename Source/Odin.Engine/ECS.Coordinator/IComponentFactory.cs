// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentFactory.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 22, 2025 5:07:38 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IComponentFactory
{
    IComponent CreateComponent(ComponentBlueprint blueprint);
}