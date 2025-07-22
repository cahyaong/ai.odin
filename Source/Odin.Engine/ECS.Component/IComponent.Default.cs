// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponent.Default.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 22, 2025 5:25:21 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public static class Component
{
    public static IComponent Unknown { get; } = UnknownComponent.Instance;
}

public class UnknownComponent : IComponent
{
    private UnknownComponent()
    {
    }

    public static UnknownComponent Instance { get; } = new();
}