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

        if (vitalityComponent.IsDead)
        {
            return;
        }

        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        intelligenceComponent.RemainingTick--;

        if (intelligenceComponent.RemainingTick > 0)
        {
            return;
        }

        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        switch (physicsComponent.MotionState)
        {
            case MotionState.Idling:
                this.HandleIdleState(entity, gameState.Universe);
                break;

            case MotionState.Walking:
            case MotionState.Running:
                this.HandleMovingState(entity);
                break;
        }

        intelligenceComponent.RemainingTick = (ushort)this._random.Next(1, 5);
    }

    private void HandleIdleState(IEntity entity, IUniverse universe)
    {
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var shouldMove = this._random.NextSingle() < 0.25f;

        if (shouldMove)
        {
            intelligenceComponent.TargetPosition = new Point
            {
                X = this._random.NextSingle() * universe.Width,
                Y = this._random.NextSingle() * universe.Height
            };

            physicsComponent.MotionState = MotionState.Walking;
        }
    }

    private void HandleMovingState(IEntity entity)
    {
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        var hasReachedTargetPosition =
            intelligenceComponent.TargetPosition == Point.Unknown ||
            physicsComponent.Position.IsCloseTo(intelligenceComponent.TargetPosition);

        if (hasReachedTargetPosition)
        {
            intelligenceComponent.TargetPosition = Point.Unknown;
            physicsComponent.MotionState = MotionState.Idling;
        }

        var shouldStop = this._random.NextSingle() < 0.25f;

        if (shouldStop)
        {
            physicsComponent.MotionState = MotionState.Idling;
        }
        else
        {
            var shouldChangeSpeed = this._random.NextSingle() < 0.5f;

            if (shouldChangeSpeed)
            {
                physicsComponent.MotionState = physicsComponent.MotionState is MotionState.Walking
                    ? MotionState.Running
                    : MotionState.Walking;
            }
        }
    }
}