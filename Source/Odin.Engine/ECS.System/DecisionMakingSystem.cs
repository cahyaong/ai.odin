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
        typeof(IntelligenceComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint _, IGameState gameState, IEntity entity)
    {
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        intelligenceComponent.RemainingTickCount--;

        if (intelligenceComponent.RemainingTickCount > 0)
        {
            return;
        }

        switch (intelligenceComponent.EntityState)
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
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var shouldMove = this._random.NextSingle() < 0.25f;

        if (shouldMove)
        {
            intelligenceComponent.TargetPosition = new Point
            {
                X = this._random.NextSingle() * universe.Width,
                Y = this._random.NextSingle() * universe.Height
            };

            intelligenceComponent.EntityState = EntityState.Walking;
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
            intelligenceComponent.EntityState = EntityState.Idle;
        }

        var shouldStop = this._random.NextSingle() < 0.25f;

        if (shouldStop)
        {
            intelligenceComponent.EntityState = EntityState.Idle;
        }
        else
        {
            var shouldChangeSpeed = this._random.NextSingle() < 0.5f;

            if (shouldChangeSpeed)
            {
                intelligenceComponent.EntityState = intelligenceComponent.EntityState == EntityState.Walking
                    ? EntityState.Running
                    : EntityState.Walking;
            }
        }
    }
}