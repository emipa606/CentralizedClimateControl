using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    class PlaceWorker_IntakeFan : PlaceWorker
    {
        /// <summary>
        /// Draw Overlay when Selected or Placing.
        /// 
        /// We draw Air Cells surrounding the Parent Object.
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

            var list = GenAdj.CellsAdjacent8Way(center, rot, size);
            GenDraw.DrawFieldEdges(list.ToList(), Color.white);
        }

        /// <summary>
        /// Place Worker for Air Intakes.
        /// 
        /// Checks:
        /// - Current Cell shouldn't have an Air Flow Pipe (Since they already have a Pipe)
        /// - Surrounding Cells from Center musn't be Impassable
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
            var list = GenAdj.CellsAdjacent8Way(center, rot, size);

            if (list.Any(intVec => intVec.Impassable(map)))
            {
                return "CentralizedClimateControl.Producer.IntakeFanPlaceError".Translate();
            }

            return true;
        }
       
    }
}
