// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecisionMakingSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Thursday, March 27, 2025 4:56:41 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

[SystemMetadata(OrderingIndex = -16)]
public class DecisionMakingSystem : BaseFixedSystem
{
    private readonly Random _random;

    public DecisionMakingSystem(IEntityManager entityManager)
        : base(entityManager)
    {
        this._random = new Random();
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(IntelligenceComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint _, IGameState gameState, IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        var isValid = vitalityComponent.EntityState is not EntityState.Unknown and not EntityState.Dead;

        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        intelligenceComponent.RemainingTickCount--;

        if (intelligenceComponent.RemainingTickCount > 0)
        {
            return;
        }

        switch (vitalityComponent.EntityState)
        {
            case EntityState.Idle:
                this.HandleIdleState(entity, gameState.Universe);
                break;

            case EntityState.Walking:
            case EntityState.Running:
                this.HandleMovingState(entity);
                break;
        }

        intelligenceComponent.RemainingTickCount = (uint)this._random.Next(1, 5);
    }

    private void HandleIdleState(IEntity entity, IUniverse universe)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var shouldMove = this._random.NextSingle() < 0.25f;

        if (shouldMove)
        {
            intelligenceComponent.TargetPosition = new Point
            {
                X = this._random.NextSingle() * universe.Width,
                Y = this._random.NextSingle() * universe.Height
            };

            vitalityComponent.EntityState = EntityState.Walking;
        }
    }

    private void HandleMovingState(IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        var hasReachedTargetPosition =
            intelligenceComponent.TargetPosition == Point.Unknown ||
            physicsComponent.Position.IsCloseTo(intelligenceComponent.TargetPosition);

        if (hasReachedTargetPosition)
        {
            intelligenceComponent.TargetPosition = Point.Unknown;
            vitalityComponent.EntityState = EntityState.Idle;
        }

        var shouldStop = this._random.NextSingle() < 0.25f;

        if (shouldStop)
        {
            vitalityComponent.EntityState = EntityState.Idle;
        }
        else
        {
            var shouldChangeSpeed = this._random.NextSingle() < 0.5f;

            if (shouldChangeSpeed)
            {
                vitalityComponent.EntityState = vitalityComponent.EntityState is EntityState.Walking
                    ? EntityState.Running
                    : EntityState.Walking;
            }
        }
    }
}