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

public partial class AppBootstrapper : Node
{
    public override void _Ready()
    {
        var container = new ContainerBuilder()
            .RegisterInfrastructure(this.GetParent())
            .RegisterSystem()
            .Build();

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
            .Register(_ => rootNode.GetNode<TimeTracker>(nameof(TimeTracker)))
            .InstancePerLifetimeScope()
            .As<ITimeTracker>();

        containerBuilder
            .Register(_ => rootNode
                .GetNode<HeadUpDisplay>(nameof(HeadUpDisplay))
                .DiagnosticRenderer)
            .InstancePerLifetimeScope()
            .As<IDiagnosticRenderer>();

        containerBuilder
            .RegisterType<GameController>()
            .InstancePerLifetimeScope()
            .As<IGameController>();

        return containerBuilder;
    }

    public static ContainerBuilder RegisterSystem(this ContainerBuilder containerBuilder)
    {
        containerBuilder
            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ISystem)) ?? Assembly.GetExecutingAssembly())
            .Where(type => type.IsAssignableTo<ISystem>())
            .InstancePerLifetimeScope()
            .As<ISystem>();

        return containerBuilder;
    }
}