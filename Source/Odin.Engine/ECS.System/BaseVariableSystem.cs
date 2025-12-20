// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseVariableSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, February 28, 2025 3:34:49 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public abstract class BaseVariableSystem : BaseSystem
{
    protected BaseVariableSystem(ScenarioBlueprint scenarioBlueprint, IEntityManager entityManager)
        : base(scenarioBlueprint, entityManager)
    {
    }

    protected abstract IReadOnlyCollection<Type> RequiredComponentTypes { get; }

    public override void ProcessVariableDuration(double delta, IGameState gameState)
    {
        this.EntityManager
            .FindEntities(this.RequiredComponentTypes)
            .ForEach(entity => this.ProcessEntity(delta, gameState, entity));
    }

    protected abstract void ProcessEntity(double delta, IGameState gameState, IEntity entity);
}