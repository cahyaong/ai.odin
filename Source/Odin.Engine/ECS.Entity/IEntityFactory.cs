// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityFactory.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 6:59:49 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IEntityFactory
{
    IUniverse CreateUniverse(float width, float height);

    IEntity CreateEntity();
}