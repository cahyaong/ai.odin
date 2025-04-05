// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityState.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, April 5, 2025 4:39:00 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public enum EntityState
{
    Unknown = 0,
    Idle,
    Walking,
    Running,
    Dead
}