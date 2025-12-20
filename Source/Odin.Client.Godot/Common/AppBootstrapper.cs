// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppBootstrapper.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:31:48 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Reflection;
using Autofac;
using nGratis.AI.Odin.Engine;
using nGratis.AI.Odin.Glue;

public partial class AppBootstrapper : Node
{
    private Node _rootNode;

    public IDataStore DataStore { get; private set; }

    public IGameController GameController { get; private set; }

    public override void _Ready()
    {
        this._rootNode = this.GetParent();

        this.Bootstrap(ScenarioBlueprint.None);
    }

    public void Bootstrap(ScenarioBlueprint scenarioBlueprint)
    {
        if (this._rootNode is null)
        {
            throw new OdinException("Application bootstrapper is NOT ready!");
        }

        var container = new ContainerBuilder()
            .RegisterBlueprint(scenarioBlueprint)
            .RegisterInfrastructure(this._rootNode)
            .RegisterStorage()
            .RegisterEntityCoordinator(this._rootNode)
            .RegisterSystem()
            .Build();

        this.DataStore = container.Resolve<IDataStore>();
        this.GameController = container.Resolve<IGameController>();
    }
}

internal static class AutofacExtensions
{
    extension(ContainerBuilder containerBuilder)
    {
        public ContainerBuilder RegisterBlueprint(ScenarioBlueprint scenarioBlueprint)
        {
            containerBuilder
                .RegisterInstance(scenarioBlueprint)
                .As<ScenarioBlueprint>();

            return containerBuilder;
        }

        public ContainerBuilder RegisterInfrastructure(Node rootNode)
        {
            containerBuilder
                .Register(_ => rootNode.GetNode<Camera>("MainCamera"))
                .InstancePerLifetimeScope()
                .As<ICamera>();

            containerBuilder
                .Register(_ => rootNode.GetNode<TimeTracker>(nameof(TimeTracker)))
                .InstancePerLifetimeScope()
                .As<ITimeTracker>();

            containerBuilder
                .Register(_ => rootNode
                    .GetNode<HeadUpDisplay>(nameof(HeadUpDisplay))
                    .StatisticsOverlay)
                .InstancePerLifetimeScope()
                .As<IStatisticsOverlay>();

            containerBuilder
                .Register(_ => rootNode.GetNode<DiagnosticsOverlay>(nameof(DiagnosticsOverlay)))
                .InstancePerLifetimeScope()
                .As<IDiagnosticOverlay>();

            containerBuilder
                .RegisterType<GameController>()
                .InstancePerLifetimeScope()
                .As<IGameController>();

            return containerBuilder;
        }

        public ContainerBuilder RegisterStorage()
        {
            containerBuilder
                .RegisterType<EmbeddedDataStore>()
                .InstancePerLifetimeScope()
                .As<IDataStore>();

            return containerBuilder;
        }

        public ContainerBuilder RegisterEntityCoordinator(Node rootNode)
        {
            containerBuilder
                .RegisterType<EntityFactory>()
                .InstancePerLifetimeScope()
                .As<IEntityFactory>();

            containerBuilder
                .RegisterType<EntityManager>()
                .InstancePerLifetimeScope()
                .As<IEntityManager>();

            containerBuilder
                .RegisterType<Engine.ComponentFactory>()
                .InstancePerLifetimeScope()
                .As<IComponentFactory>();

            containerBuilder
                .Register(_ => (ComponentFactory)rootNode.FindChild(nameof(ComponentFactory)))
                .InstancePerLifetimeScope()
                .As<IComponentFactory>();

            return containerBuilder;
        }

        public ContainerBuilder RegisterSystem()
        {
            containerBuilder
                .RegisterAssemblyTypes(
                    Assembly.GetAssembly(typeof(ISystem)) ?? Assembly.GetExecutingAssembly(),
                    Assembly.GetExecutingAssembly())
                .Where(type => type.IsAssignableTo<ISystem>())
                .InstancePerLifetimeScope()
                .As<ISystem>();

            return containerBuilder;
        }
    }
}