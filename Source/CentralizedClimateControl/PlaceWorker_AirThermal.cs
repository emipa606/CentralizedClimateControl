using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl;

public class PlaceWorker_AirThermal : PlaceWorker
{
    /// <summary>
    ///     Draw Overlay when Selected or Placing.
    ///     Here we just draw a red cell towards the South. To indicate Exhaust.
    /// </summary>
    /// <param name="def">The Thing's Def</param>
    /// <param name="center">Location</param>
    /// <param name="rot">Rotation</param>
    /// <param name="ghostCol">Ghost Color</param>
    /// <param name="thing"></param>
    public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
    {
        if (def == null)
        {
            return;
        }

        var list = new List<IntVec3>();

        var iterator = new IntVec3(center.x, center.y, center.z);

        for (var dx = 0; dx < def.size.x; dx++)
        {
            list.Add(iterator + IntVec3.South.RotatedBy(rot));

            iterator += IntVec3.East.RotatedBy(rot);
        }

        GenDraw.DrawFieldEdges(list, Color.red);
    }

    /// <summary>
    ///     Place Worker for Climate Units.
    ///     Checks:
    ///     - Current Cell shouldn't have an Air Flow Pipe (Since they already have a Pipe)
    ///     - South Cell from Center must not be Impassable
    /// </summary>
    /// <param name="checkingDef">The Def Being Built</param>
    /// <param name="loc">Target Location</param>
    /// <param name="rot">Rotation of the Object to be Placed</param>
    /// <param name="map"></param>
    /// <param name="thingToIgnore">Unused field</param>
    /// <param name="thing"></param>
    /// <returns>Boolean/Acceptance Report if we can place the object of not.</returns>
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map,
        Thing thingToIgnore = null, Thing thing = null)
    {
        if (loc.GetThingList(map).OfType<Building_AirPipe>().Any() || checkingDef == null)
        {
            return AcceptanceReport.WasRejected;
        }

        var iterator = new IntVec3(loc.x, loc.y, loc.z);

        for (var dx = 0; dx < checkingDef.Size.x; dx++)
        {
            var cellToTest = iterator + IntVec3.South.RotatedBy(rot);
            if (cellToTest.InBounds(map) && cellToTest.Impassable(map))
            {
                return "CentralizedClimateControl.Consumer.AirThermalPlaceError".Translate();
            }

            iterator += IntVec3.East.RotatedBy(rot);
        }

        return true;
    }
}