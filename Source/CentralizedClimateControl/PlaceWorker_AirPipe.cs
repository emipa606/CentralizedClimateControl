﻿using System.Linq;
using Verse;

namespace CentralizedClimateControl;

public class PlaceWorker_AirPipe : PlaceWorker
{
    /// <summary>
    ///     Place Worker for Air Pipes. Checks if Air Pipes are in a Suitable Location or not.
    ///     Checks:
    ///     - Current Cell shouldn't have an Air Flow Building (Since they already have a Pipe)
    ///     - If a pipe of the same color is built on the tile, don't allow it
    /// </summary>
    /// <param name="def">The Def Being Built</param>
    /// <param name="loc">Target Location</param>
    /// <param name="rot">Rotation of the Object to be Placed</param>
    /// <param name="map"></param>
    /// <param name="thingToIgnore">Unused field</param>
    /// <param name="thing"></param>
    /// <returns>Boolean/Acceptance Report if we can place the object of not.</returns>
    public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map,
        Thing thingToIgnore = null, Thing thing = null)
    {
        //var thingList = loc.GetThingList(map);
        //return thingList.OfType<Building_AirFlowControl>().Any() ? AcceptanceReport.WasRejected : AcceptanceReport.WasAccepted;
        if (loc.GetThingList(map).OfType<Building_AirFlowControl>().Any())
        {
            return AcceptanceReport.WasRejected;
        }

        var pipeBaseName = def.defName.Replace("Hidden", "");
        return loc.GetThingList(map).OfType<Building_AirPipe>().Any(pipe => pipe.def.defName.Contains(pipeBaseName))
            ? AcceptanceReport.WasRejected
            : AcceptanceReport.WasAccepted;
    }
}