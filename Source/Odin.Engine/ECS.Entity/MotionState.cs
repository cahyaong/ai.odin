// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MotionState.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, April 5, 2025 4:39:00 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public enum MotionState
{
    Unknown = 0,

    Idling,
    Walking,
    Running,

    Immobilized
}