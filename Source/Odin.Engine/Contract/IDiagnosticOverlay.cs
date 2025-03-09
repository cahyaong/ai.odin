// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDiagnosticOverlay.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, March 8, 2025 5:51:40 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IDiagnosticOverlay
{
    void Update(IGameState gameState);
}