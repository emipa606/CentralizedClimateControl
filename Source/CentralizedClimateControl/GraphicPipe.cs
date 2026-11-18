using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl;

public class GraphicPipe : Graphic_Linked
{
    private readonly AirFlowType FlowType;

    public GraphicPipe()
    {
    }

    /// <summary>
    ///     Graphic for Pipes Constructor
    /// </summary>
    /// <param name="graphic">Multi Graphic Object</param>
    /// <param name="flowType">Type of Pipe</param>
    public GraphicPipe(Graphic graphic, AirFlowType flowType)
    {
        subGraphic = graphic;
        FlowType = flowType;
    }

    /// <summary>
    ///     Graphic for Pipes Constructor with Defaulted Red Pipe
    /// </summary>
    /// <param name="graphic">Multi Graphic Object</param>
    public GraphicPipe(Graphic graphic)
    {
        subGraphic = graphic;
        FlowType = AirFlowType.Hot;
    }

    /// <summary>
    ///     Overriden Function for Pipe Atlas. It Checks for Neighbouring tiles if it should be Linked to the target cell.
    ///     This Function specifies the condition that will be used.
    ///     Here we just check if the target cell that is asked for linkage has a Pipe of the same Color or not.
    /// </summary>
    /// <param name="c">Target Cell</param>
    /// <param name="parent">Parent Object</param>
    /// <returns>Should Link with Same Color Pipe or not</returns>
    public override bool ShouldLinkWith(IntVec3 c, Thing parent)
    {
        return c.InBounds(parent.Map) &&
               CentralizedClimateControlUtility.GetNetManager(parent.Map).ZoneAt(c, FlowType);
    }


    /// <summary>
    ///     Main method to Print an Atlas Pipe Graphic
    /// </summary>
    /// <param name="layer">Section Layer calling this Print command</param>
    /// <param name="thing">Parent Object</param>
    /// <param name="extraRotation"></param>
    public override void Print(SectionLayer layer, Thing thing, float extraRotation)
    {
        if (thing.def.defName.Contains("Hidden"))
        {
            return;
        }

        Printer_Plane.PrintPlane(
            layer,
            thing.TrueCenter(),
            Vector2.one,
            LinkedDrawMatFrom(thing, thing.Position)
        );

        for (var i = 0; i < 4; i++)
        {
            var intVec = thing.Position + GenAdj.CardinalDirections[i];

            if (!intVec.InBounds(thing.Map) ||
                !CentralizedClimateControlUtility.GetNetManager(thing.Map).ZoneAt(intVec, FlowType) ||
                intVec.GetTerrain(thing.Map).layerable)
            {
                continue;
            }

            if (intVec.GetThingList(thing.Map).OfType<Building_AirPipe>().Any())
            {
                continue;
            }

            Printer_Plane.PrintPlane(
                layer,
                intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude),
                Vector2.one,
                LinkedDrawMatFrom(thing, intVec));
        }
    }
}