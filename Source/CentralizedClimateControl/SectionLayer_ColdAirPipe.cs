﻿using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl;

internal class SectionLayer_ColdAirPipe : SectionLayer_Things
{
    public readonly AirFlowType FlowType;

    /// <summary>
    ///     Blue Pipe Overlay Section Layer
    /// </summary>
    /// <param name="section">Section of the Map</param>
    public SectionLayer_ColdAirPipe(Section section) : base(section)
    {
        FlowType = AirFlowType.Cold;
        requireAddToMapMesh = false;
        relevantChangeTypes = MapMeshFlagDefOf.Things;
    }

    /// <summary>
    ///     Function which Checks if we need to Draw the Layer or not. If we do, we call the Base DrawLayer();
    /// </summary>
    public override void DrawLayer()
    {
        var designatorBuild = Find.DesignatorManager.SelectedDesignator as Designator_Build;

        var thingDef = designatorBuild?.PlacingDef as ThingDef;

        if (thingDef?.comps.OfType<CompProperties_AirFlow>().FirstOrDefault(x => x.flowType == FlowType) != null)
        {
            base.DrawLayer();
        }
    }

    /// <summary>
    ///     Called when a Draw is initiated from DrawLayer.
    /// </summary>
    /// <param name="thing">Thing that triggered the Draw Call</param>
    protected override void TakePrintFrom(Thing thing)
    {
        if (thing is not Building building)
        {
            return;
        }

        var compAirFlow = building.GetComps<CompAirFlow>()
            .FirstOrDefault(x => x.FlowType == FlowType || x.FlowType == AirFlowType.Any);
        compAirFlow?.PrintForGrid(this, FlowType);
    }
}