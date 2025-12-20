// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimulationController.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, August 29, 2025 6:55:32 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Collections.Immutable;
using nGratis.AI.Odin.Engine;

public partial class SimulationController : HBoxContainer
{
    private AppBootstrapper _appBootstrapper;

    private ImmutableList<ScenarioBlueprint> _scenarioBlueprints;

    private ScenarioBlueprint _selectedScenarioBlueprint;

    protected OptionButton ScenarioSelector =>
        field ??=
        this.GetNode<OptionButton>(nameof(SimulationController.ScenarioSelector));

    protected Button RunningButton =>
        field ??=
        this.GetNode<Button>(nameof(SimulationController.RunningButton));

    public override void _Ready()
    {
        this._appBootstrapper = this
            .FindAncestor<HeadUpDisplay>()
            .AppBootstrapper;

        this._scenarioBlueprints = this
            ._appBootstrapper.DataStore
            .LoadScenarioBlueprints()
            .ToImmutableList();

        this._scenarioBlueprints
            .ForEach(scenarioBlueprint => this.ScenarioSelector.AddItem(scenarioBlueprint.Id));

        this.ScenarioSelector.ItemSelected += this.OnScenarioSelected;
        this.OnScenarioSelected(0);

        this.RunningButton.Pressed += this.OnRunningButtonPressed;
    }

    private void OnScenarioSelected(long index)
    {
        var adjustedIndex = int.Min((int)index, this._scenarioBlueprints.Count - 1);
        this._selectedScenarioBlueprint = this._scenarioBlueprints[adjustedIndex];
    }

    private void OnRunningButtonPressed()
    {
        // TODO (MUST): Implement state clearing logic when switching scenario!

        this._appBootstrapper.GameController?.End();
        this._appBootstrapper.Bootstrap(this._selectedScenarioBlueprint);
        this._appBootstrapper.GameController?.Start();
    }
}