using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    public class Building_IntakeFan : Building_AirFlowControl
    {
        private int _windCellsBlocked = 0;
        private const float EfficiencyLossPerWindCubeBlocked = 0.0076923077f;

        public CompAirFlowProducer CompAirProducer;

        /// <summary>
        /// Building spawned on the map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <param name="respawningAfterLoad">Unused flag</param>
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CompAirProducer = GetComp<CompAirFlowProducer>();
            CompAirProducer.Props.flowType = AirFlowType.Any;
        }

        /// <summary>
        /// Tick Intake Fan. Check the surrondings and generate Air Flow if all clear.
        /// </summary>
        public override void TickRare()
        {
            if (!CompPowerTrader.PowerOn)
            {
                CompAirProducer.IsPoweredOff = true;
                CompAirProducer.CurrentAirFlow = 0;
                return;
            }

            CompAirProducer.IsPoweredOff = false;
            CompAirProducer.IsBrokenDown = this.IsBrokenDown();

            var sumTemp = 0f;

            var size = def.Size;
            var list = GenAdj.CellsAdjacent8Way(Position, Rotation, size).ToList();

            foreach (var intVec in list)
            {
                if (intVec.Impassable(Map))
                {
                    CompAirProducer.CurrentAirFlow = 0;
                    CompAirProducer.IsBlocked = true;
                    return;
                }

                sumTemp += intVec.GetTemperature(Map);
            }

            CompAirProducer.IsBlocked = false;

            if (!CompAirProducer.IsActive())
            {
                return;
            }

            var intake = sumTemp / list.Count;
            CompAirProducer.IntakeTemperature = intake;

            var flow = CompAirProducer.Props.baseAirFlow - _windCellsBlocked * EfficiencyLossPerWindCubeBlocked;
            CompAirProducer.CurrentAirFlow = flow;
        }
    }
}
