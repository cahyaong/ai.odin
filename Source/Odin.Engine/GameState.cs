// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameState.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 7:20:39 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class GameState : IGameState
{
    public required IUniverse Universe { get; init; }
}