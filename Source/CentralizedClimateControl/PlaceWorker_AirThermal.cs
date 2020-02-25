using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class PlaceWorker_AirThermal: PlaceWorker
    {
        /// <summary>
        /// Draw Overlay when Selected or Placing.
        /// 
        /// Here we just draw a red cell towards the South. To indicate Exhaust.
        /// </summary>
        /// <param name="def">The Thing's Def</param>
        /// <param name="center">Location</param>
        /// <param name="rot">Rotation</param>
        /// <param name="ghostCol">Ghost Color</param>
        
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            if (def == null)
            {
                return;
            }

            var size = def.size;

            List<IntVec3> list = new List<IntVec3>();

            IntVec3 iterator = new IntVec3(center.x, center.y, center.z);

            for (int dx = 0; dx < size.x; dx++)
            {
                IntVec3 intVec = iterator + IntVec3.South.RotatedBy(rot);
                list.Add(intVec);

                iterator += IntVec3.East.RotatedBy(rot);
            }

            GenDraw.DrawFieldEdges(list, Color.red);
        }

        /// <summary>
        /// Place Worker for Climate Units.
        /// 
        /// Checks:
        /// - Current Cell shouldn't have an Air Flow Pipe (Since they already have a Pipe)
        /// - South Cell from Center musn't be Impassable
        /// </summary>
        /// <param name="def">The Def Being Built</param>
        /// <param name="center">Target Location</param>
        /// <param name="rot">Rotation of the Object to be Placed</param>
        /// <param name="thingToIgnore">Unused field</param>
        /// <returns>Boolean/Acceptance Report if we can place the object of not.</returns>
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            var thingList = center.GetThingList(map);

            if (thingList.OfType<Building_AirPipe>().Any())
            {
                return AcceptanceReport.WasRejected;
            }

            if (def == null)
            {
                return AcceptanceReport.WasRejected;
            }

            var size = def.Size;

            var iterator = new IntVec3(center.x, center.y, center.z);

            for (var dx = 0; dx < size.x; dx++)
            {
                var intVec = iterator + IntVec3.South.RotatedBy(rot);

                if (intVec.Impassable(map))
                {
                    return "CentralizedClimateControl.Consumer.AirThermalPlaceError".Translate();
                }

                iterator += IntVec3.East.RotatedBy(rot);
            }

            return true;
        }
    }
}
