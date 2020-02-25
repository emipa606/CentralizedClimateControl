using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    class SectionLayer_HotAirPipe : SectionLayer_Things
    {
        public AirFlowType FlowType;

        /// <summary>
        /// Red Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_HotAirPipe(Section section) : base(section)
        {
            FlowType = AirFlowType.Hot;
            requireAddToMapMesh = false;
            relevantChangeTypes = (MapMeshFlag) 4;
        }

        /// <summary>
        /// Function which Checks if we need to Draw the Layer or not. If we do, we call the Base DrawLayer();
        /// 
        /// We Check if the Pipe is a Red Pipe and thus start a DrawLayer request.
        /// </summary>
        public override void DrawLayer()
        {
            var designatorBuild = Find.DesignatorManager.SelectedDesignator as Designator_Build;

            var thingDef = designatorBuild?.PlacingDef as ThingDef;

            if (thingDef?.comps.OfType<CompProperties_AirFlow>().FirstOrDefault((x) => x.flowType == FlowType) != null)
            {
                base.DrawLayer();
            }
        }

        /// <summary>
        /// Called when a Draw is initiated from DrawLayer.
        /// </summary>
        /// <param name="thing">Thing that triggered the Draw Call</param>
        protected override void TakePrintFrom(Thing thing)
        {
            var building = thing as Building;
            if (building == null)
            {
                return;
            }

            var compAirFlow = building.GetComps<CompAirFlow>().FirstOrDefault((x) => x.FlowType == FlowType || x.FlowType == AirFlowType.Any);
            compAirFlow?.PrintForGrid(this, FlowType);
        }
    }
}
