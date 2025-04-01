// --------------------------------------------------------------------------------------------------------------------
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

    private uint _framePerSecond;
    private double _accumulatedDelta;

    public DebuggingSystem(
        IStatisticsOverlay statisticsOverlay,
        IDiagnosticOverlay diagnosticOverlay,
        IEntityManager entityManager)
        : base(entityManager)
    {
        this._statisticsOverlay = statisticsOverlay;
        this._diagnosticOverlay = diagnosticOverlay;

        this._framePerSecond = 0;
        this._accumulatedDelta = 0.0;
    }

    public override void ProcessVariableDuration(double delta, IGameState gameState)
    {
        this._framePerSecond++;
        this._accumulatedDelta += delta;

        if (this._accumulatedDelta >= 1.0)
        {
            this._statisticsOverlay.Update(gameState
                .DebuggingStatistics
                .AddMetric("FPS", this._framePerSecond));

            this._framePerSecond = 0;
            this._accumulatedDelta--;
        }
    }

    public override void ProcessFixedDuration(uint tick, IGameState gameState)
    {
        this._statisticsOverlay.Update(gameState
            .DebuggingStatistics
            .AddMetric("Tick", tick)
            .AddMetric("Entity Count", this.EntityManager.TotalCount));

        this._diagnosticOverlay.Update(gameState);
    }
}