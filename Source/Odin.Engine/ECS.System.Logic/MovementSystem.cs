// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, March 30, 2025 1:05:08 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class MovementSystem : BaseFixedSystem
{
    private static readonly float MaxWalkingSpeed = 2.0f;
    private static readonly float MaxRunningSpeed = 4.0f;
    private readonly Random _random;

    public MovementSystem(IEntityManager entityManager)
        : base(ScenarioBlueprint.None, entityManager)
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

        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        if (!physicsComponent.IsMoving)
        {
            return;
        }

        var maxSpeed = physicsComponent.MotionState is MotionState.Walking
            ? MaxWalkingSpeed
            : MaxRunningSpeed;

        var velocity = new Vector
        {
            X = this._random.NextSingle() * maxSpeed,
            Y = this._random.NextSingle() * maxSpeed
        };

        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var deltaPosition = intelligenceComponent.TargetPosition - physicsComponent.Position;

        physicsComponent.Velocity = deltaPosition.Sign() * velocity;
        physicsComponent.Position += physicsComponent.Velocity;
        physicsComponent.Position = physicsComponent.Position.Clamp(gameState.Universe.Size);
    }
}