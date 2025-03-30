﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebuggingSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:24:05 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

[SystemMetadata(OrderingIndex = int.MaxValue)]
public class DebuggingSystem : BaseSystem
{
    private readonly IStatisticsOverlay _statisticsOverlay;
    private readonly IDiagnosticOverlay _diagnosticOverlay;

    public DebuggingSystem(
        IStatisticsOverlay statisticsOverlay,
        IDiagnosticOverlay diagnosticOverlay,
        IEntityManager entityManager)
        : base(entityManager)
    {
        this._statisticsOverlay = statisticsOverlay;
        this._diagnosticOverlay = diagnosticOverlay;
    }

    public override void ProcessFixedDuration(uint tick, IGameState gameState)
    {
        this._statisticsOverlay.UpdateMetric("Tick", tick.ToString());
        this._statisticsOverlay.UpdateMetric("Entity Count", this.EntityManager.TotalCount.ToString("N0"));

        this._diagnosticOverlay.Update(gameState);
    }
}