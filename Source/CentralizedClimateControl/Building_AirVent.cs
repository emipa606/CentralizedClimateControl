using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class Building_AirVent : Building
    {
        public CompAirFlowConsumer CompAirFlowConsumer;

        /// <summary>
        /// Building spawned on the map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <param name="respawningAfterLoad">Unused flag</param>
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CompAirFlowConsumer = GetComp<CompAirFlowConsumer>();
        }

        /// <summary>
        /// Get the Gizmos for AirVent
        /// Here, we generate the Gizmo for Chaning Pipe Priority
        /// </summary>
        /// <returns>List of Gizmos</returns>
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }

            if (CompAirFlowConsumer != null)
            {
                yield return CentralizedClimateControlUtility.GetPipeSwitchToggle(CompAirFlowConsumer);
            }
        }

        /// <summary>
        /// Tick function for Air Vents
        /// Main code for chaning temperature at the Rooms. We take the Converted Temperature from the Air Network.
        /// </summary>
        public override void TickRare()
        {
            CompAirFlowConsumer.TickRare();

            if (!CompAirFlowConsumer.IsOperating())
            {
                return;
            }

            if (!CompAirFlowConsumer.IsActive())
            {
                return;
            }

            var outsideTemp = CompAirFlowConsumer.ConvertedTemperature;

            IntVec3 intVec = Position + IntVec3.North.RotatedBy(Rotation);

            if (intVec.Impassable(Map))
            {
                return;
            }

            var insideTemp = intVec.GetTemperature(Map);
            var tempDiff = outsideTemp - insideTemp;
            var magnitudeChange = Mathf.Abs(tempDiff);

            // Cap change at 10.0f
            if (magnitudeChange > 10.0f)
            {
                magnitudeChange = 10.0f;
            }

            float signChanger = 1;

            if (tempDiff < 0)
            {
                signChanger = -1;
            }

            var thermalFlag = 1;

            if (CompAirFlowConsumer.ThermalEfficiency <= 0)
            {
                thermalFlag = 0;
            }

            // Flow Efficiency is capped at 1.0f. Squaring will only keep it less than or equal to 1.0f. Smaller the number more drastic the square.
            var efficiencyImpact = CompAirFlowConsumer.FlowEfficiency * CompAirFlowConsumer.FlowEfficiency * thermalFlag;

            var smoothMagnitude =  magnitudeChange * 0.25f * (CompAirFlowConsumer.Props.baseAirExhaust / 100.0f);
            var energyLimit = smoothMagnitude * efficiencyImpact * 4.16666651f * 12f * signChanger;
            var tempChange = GenTemperature.ControlTemperatureTempChange(intVec, Map, energyLimit, outsideTemp);
            
            var flag = !Mathf.Approximately(tempChange, 0f);
            if (flag)
            {
                intVec.GetRoomGroup(Map).Temperature += tempChange;
            }
        }
    }
}
