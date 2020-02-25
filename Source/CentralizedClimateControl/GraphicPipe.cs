using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace CentralizedClimateControl
{
    public class GraphicPipe : Graphic_Linked
    {
        public AirFlowType FlowType;

        public GraphicPipe()
        {
        }

        /// <summary>
        /// Graphic for Pipes Constructor
        /// </summary>
        /// <param name="graphic">Multi Graphic Object</param>
        /// <param name="flowType">Type of Pipe</param>
        public GraphicPipe(Graphic graphic, AirFlowType flowType)
        {
            subGraphic = graphic;
            FlowType = flowType;
        }

        /// <summary>
        /// Graphic for Pipes Constructor with Defaulted Red Pipe
        /// </summary>
        /// <param name="graphic">Multi Graphic Object</param>
        public GraphicPipe(Graphic graphic)
        {
            subGraphic = graphic;
            FlowType = AirFlowType.Hot;
        }

        /// <summary>
        /// Overriden Function for Pipe Atlas. It Checks for Neighbouring tiles if it should be Linked to the target cell.
        /// This Function specifies the condition that will be used.
        /// 
        /// Here we just check if the target cell that is asked for linkage has a Pipe of the same Color or not.
        /// </summary>
        /// <param name="vec">Target Cell</param>
        /// <param name="parent">Parent Object</param>
        /// <returns>Should Link with Same Color Pipe or not</returns>
        public override bool ShouldLinkWith(IntVec3 vec, Thing parent)
        {
            return vec.InBounds(parent.Map) && CentralizedClimateControlUtility.GetNetManager(parent.Map).ZoneAt(vec, FlowType);
        }

        /// <summary>
        /// Predicate to check if Object is a Pipe Building
        /// </summary>
        /// <param name="obj">Game Object</param>
        /// <returns>True if type is Building_Pipe</returns>
        private static bool CheckPipe(Thing obj)
        {
            return obj.GetType() == typeof(Building_AirPipe);
        }

        /// <summary>
        /// Main method to Print a Atlas Pipe Graphic
        /// </summary>
        /// <param name="layer">Section Layer calling this Print command</param>
        /// <param name="parent">Parent Object</param>
        public override void Print(SectionLayer layer, Thing parent)
        {
            var material = LinkedDrawMatFrom(parent, parent.Position);
            Printer_Plane.PrintPlane(layer, parent.TrueCenter(), Vector2.one, material, 0f);
            for (var i = 0; i < 4; i++)
            {
                var intVec = parent.Position + GenAdj.CardinalDirections[i];

                if (!intVec.InBounds(parent.Map) ||
                    !CentralizedClimateControlUtility.GetNetManager(parent.Map).ZoneAt(intVec, FlowType) || intVec.GetTerrain(parent.Map).layerable)
                {
                    continue;
                }

                var thingList = intVec.GetThingList(parent.Map);
            
                Predicate<Thing> predicate = CheckPipe;
                if (thingList.Any(predicate))
                {
                    continue;
                }

                var material2 = LinkedDrawMatFrom(parent, intVec);
                Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(parent.def.Altitude), Vector2.one, material2, 0f);
            }
        }
    }
}
