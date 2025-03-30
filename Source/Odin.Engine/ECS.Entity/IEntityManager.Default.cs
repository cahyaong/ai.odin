// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityManager.Default.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, March 30, 2025 2:13:01 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public sealed class UnknownEntityManager : IEntityManager
{
    private UnknownEntityManager()
    {
    }

    public static UnknownEntityManager Instance { get; } = new();

    public uint TotalCount => 0;

    public void AddEntity(IEntity _) =>
        throw new NotSupportedException("Adding entity is not allowed!");

    public IReadOnlyCollection<IEntity> FindEntities(params IReadOnlyCollection<Type> _) =>
        throw new NotSupportedException("Finding entities is not allowed!");
}