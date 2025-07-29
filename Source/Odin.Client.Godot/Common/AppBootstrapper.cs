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
    public IDataStore DataStore { get; private set; }

    public override void _Ready()
    {
        var rootNode = this.GetParent();

        var container = new ContainerBuilder()
            .RegisterInfrastructure(rootNode)
            .RegisterStorage()
            .RegisterEntityCoordinator(rootNode)
            .RegisterSystem()
            .Build();

        this.DataStore = container
            .Resolve<IDataStore>();

        container
            .Resolve<IGameController>()
            .Start();
    }
}

internal static class AutofacExtensions
{
    public static ContainerBuilder RegisterInfrastructure(this ContainerBuilder containerBuilder, Node rootNode)
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

    public static ContainerBuilder RegisterStorage(this ContainerBuilder containerBuilder)
    {
        containerBuilder
            .RegisterType<EmbeddedDataStore>()
            .InstancePerLifetimeScope()
            .As<IDataStore>();

        return containerBuilder;
    }

    public static ContainerBuilder RegisterEntityCoordinator(this ContainerBuilder containerBuilder, Node rootNode)
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

    public static ContainerBuilder RegisterSystem(this ContainerBuilder containerBuilder)
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