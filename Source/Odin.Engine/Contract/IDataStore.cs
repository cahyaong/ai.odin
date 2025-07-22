// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataStore.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, May 28, 2025 3:02:39 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IDataStore
{
    IEnumerable<EntityBlueprint> LoadEntityBlueprints();
}