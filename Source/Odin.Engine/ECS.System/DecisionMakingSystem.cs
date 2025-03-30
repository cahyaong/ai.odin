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
        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        var shouldUpdateTargetPosition =
            intelligenceComponent.TargetPosition == Point.Unknown ||
            physicsComponent.Position.IsCloseTo(intelligenceComponent.TargetPosition);

        if (shouldUpdateTargetPosition)
        {
            intelligenceComponent.TargetPosition = new Point
            {
                X = this._random.NextSingle() * gameState.Universe.Width,
                Y = this._random.NextSingle() * gameState.Universe.Height
            };
        }
    }
}