﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, February 28, 2025 3:34:49 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public abstract class BaseSystem : ISystem
{
    public virtual void ProcessVariableDuration(double delta, IGameState gameState)
    {
    }

    public virtual void ProcessFixedDuration(uint tick, IGameState gameState)
    {
    }
}