using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class GraphicPipe_Overlay: Graphic_Linked
    {
        public AirFlowType FlowType;

        private readonly Graphic _anyGraphic;
        private readonly Graphic _flowGraphic;

        public GraphicPipe_Overlay()
        {
        }

        /// <summary>
        /// Graphic for Overlay Pipes Constructor. Defaults to Red Pipe as FlowType.
        /// </summary>
        /// <param name="subGraphic">Color Specific Overlay</param>
        /// <param name="anyGraphic">Any Pipe Overlay Graphic</param>
        public GraphicPipe_Overlay(Graphic subGraphic, Graphic anyGraphic) : base(subGraphic)
        {
            FlowType = AirFlowType.Hot;
            _anyGraphic = anyGraphic;
            _flowGraphic = subGraphic;
        }

        /// <summary>
        /// Graphic for Overlay Pipes Constructor
        /// </summary>
        /// <param name="subGraphic">Color Specific Overlay</param>
        /// <param name="anyGraphic">Any Pipe Overlay Graphic</param>
        /// <param name="type">Flow Type of the Atlas</param>
        public GraphicPipe_Overlay(Graphic subGraphic, Graphic anyGraphic, AirFlowType type) : base(subGraphic)
        {
            FlowType = type;
            _anyGraphic = anyGraphic;
            _flowGraphic = subGraphic;
        }

        /// <summary>
        /// Overriden Function for Pipe Atlas. It Checks for Neighbouring tiles if it should be Linked to the target cell.
        /// This Function specifies the condition that will be used.
        /// 
        /// Here we just check if the target cell that is asked for linkage has a Pipe of the same Color or not.
        /// </summary>
        /// <param name="intVec">Target Cell</param>
        /// <param name="parent">Parent Object</param>
        /// <returns>Should Link with Same Color Pipe or not</returns>
        public override bool ShouldLinkWith(IntVec3 intVec, Thing parent)
        {
            var building = parent as Building;
            if (building == null)
            {
                return false;
            }

            return intVec.InBounds(parent.Map) && CentralizedClimateControlUtility.GetNetManager(parent.Map).ZoneAt(intVec, FlowType);
        }

        /// <summary>
        /// Main method to Print a Atlas Pipe Graphic
        /// </summary>
        /// <param name="layer">Section Layer calling this Print command</param>
        /// <param name="parent">Parent Object</param>
        public override void Print(SectionLayer layer, Thing parent)
        {
            var iterator = parent.OccupiedRect().GetIterator();
            while (!iterator.Done())
            {
                var current = iterator.Current;
                var vector = current.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);

                var building = parent as Building;

                var compAirFlow = building?.GetComps<CompAirFlow>().FirstOrDefault();
                if (compAirFlow == null)
                {
                    return;
                }

                if (compAirFlow.FlowType != FlowType && compAirFlow.FlowType != AirFlowType.Any)
                {
                    return;
                }

                subGraphic = compAirFlow.FlowType == AirFlowType.Any ? _anyGraphic : _flowGraphic;

                Printer_Plane.PrintPlane(layer, vector, Vector2.one, LinkedDrawMatFrom(parent, current), 0f);
                iterator.MoveNext();
            }
        }
    }
}
