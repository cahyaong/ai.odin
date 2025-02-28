// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:24:05 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class DiagnosticSystem : BaseSystem
{
    private readonly IDiagnosticRenderer _diagnosticRenderer;

    public DiagnosticSystem(IDiagnosticRenderer diagnosticRenderer)
    {
        this._diagnosticRenderer = diagnosticRenderer;
    }

    public override void ProcessFixedDuration(uint tick, IGameState gameState)
    {
        this._diagnosticRenderer.UpdateStatistic("Tick", tick.ToString());
    }
}