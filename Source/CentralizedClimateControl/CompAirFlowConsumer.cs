using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompAirFlowConsumer : CompAirFlow
    {
        public const string AirFlowOutputKey = "CentralizedClimateControl.AirFlowOutput";
        public const string IntakeTempKey = "CentralizedClimateControl.Consumer.ConvertedTemperature";
        public const string FlowEfficiencyKey = "CentralizedClimateControl.Consumer.FlowEfficiencyKey";
        public const string ThermalEfficiencyKey = "CentralizedClimateControl.Consumer.ThermalEfficiencyKey";
        public const string DisconnectedKey = "CentralizedClimateControl.Consumer.Disconnected";
        public const string ClosedKey = "CentralizedClimateControl.Consumer.Closed";

        public float ConvertedTemperature;
        protected CompFlickable FlickableComp;

        private bool _alertChange;
        public AirTypePriority AirTypePriority = AirTypePriority.Auto;

        public float ExhaustAirFlow
        {
            get
            {
                return Props.baseAirExhaust;
            }
        }

        public float FlowEfficiency
        {
            get
            {
                return AirFlowNet.FlowEfficiency;
            }
        }

        public float ThermalEfficiency
        {
            get
            {
                return AirFlowNet.ThermalEfficiency;
            }
        }

        /// <summary>
        /// Debug String for AirFlow Consumer
        /// </summary>
        public string DebugString
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(parent.LabelCap + " CompAirFlowConsumer:");
                stringBuilder.AppendLine("   ConvertedTemperature: " + ConvertedTemperature);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Post Spawn for Component
        /// </summary>
        /// <param name="respawningAfterLoad">Unused Flag</param>
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterConsumer(this);
            FlickableComp = parent.GetComp<CompFlickable>();

            base.PostSpawnSetup(respawningAfterLoad);
        }

        /// <summary>
        /// Method called during Game Save/Load
        /// </summary>
        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref AirTypePriority, "airTypePriority", AirTypePriority.Auto);
#if DEBUG
            Debug.Log(parent + " - Air Priority Loaded: " + AirTypePriority);
#endif
            _alertChange = true;
        }

        /// <summary>
        /// Component De-spawned from Map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        public override void PostDeSpawn(Map map)
        {
            CentralizedClimateControlUtility.GetNetManager(map).DeregisterConsumer(this);
            ResetFlowVariables();
            base.PostDeSpawn(map);
        }

        /// <summary>
        /// Extra Component Inspection string
        /// </summary>
        /// <returns>String Containing information for Consumers</returns>
        public override string CompInspectStringExtra()
        {
            if (!FlickableComp.SwitchIsOn)
            {
                return ClosedKey.Translate() + "\n" + base.CompInspectStringExtra();
            }

            if (!IsOperating())
            {
                return base.CompInspectStringExtra();
            }

            if (!IsActive())
            {
                return DisconnectedKey.Translate() + "\n" + base.CompInspectStringExtra();
            }

            var convertedTemp = ConvertedTemperature.ToStringTemperature("F0");
            var str = IntakeTempKey.Translate(convertedTemp);

            var flowPercent = Mathf.FloorToInt(AirFlowNet.FlowEfficiency * 100) + "%";
            str += "\n";
            str += FlowEfficiencyKey.Translate(flowPercent);

            var thermalPercent = Mathf.FloorToInt(AirFlowNet.ThermalEfficiency * 100) + "%";
            str += "\n";
            str += ThermalEfficiencyKey.Translate(thermalPercent);

            return str + "\n" + base.CompInspectStringExtra();
        }

        /// <summary>
        /// Set the Pipe Priority for Consumers
        /// </summary>
        /// <param name="priority">Priority to Switch to.</param>
        public void SetPriority(AirTypePriority priority)
        {
            _alertChange = true;
            AirTypePriority = priority;
            AirFlowNet = null;
#if DEBUG
            Debug.Log("Setting Priority to: " + AirTypePriority);
#endif
        }

        /// <summary>
        /// Tick for Consumers. Here:
        /// - We Rebuild if Priority is Changed
        /// - We take the Converted Temperature from Climate Units
        /// </summary>
        public void TickRare()
        {
            if (_alertChange)
            {
                var manager = CentralizedClimateControlUtility.GetNetManager(parent.Map);
                manager.DirtyPipeWholeGrid();
                _alertChange = false;
            }

            if (!IsOperating())
            {
                return;
            }

            ConvertedTemperature = AirFlowNet.AverageConvertedTemperature;
        }

        public override bool IsOperating()
        {
            if (!FlickableComp.SwitchIsOn)
            {
                return false;
            }

            return base.IsOperating();
        }

        /// <summary>
        /// Reset the Flow Variables and Forward the Control to Base class for more reset.
        /// </summary>
        public override void ResetFlowVariables()
        {
            ConvertedTemperature = 0.0f;
            base.ResetFlowVariables();
        }

        /// <summary>
        /// Check if Consumer Can work.
        /// This check is used after checking for Power.
        /// </summary>
        /// <returns>Boolean flag to show if Active</returns>
        public bool IsActive()
        {
            if (AirFlowNet == null)
            {
                return false;
            }

            return AirFlowNet.Producers.Count != 0 && AirFlowNet.Consumers.Count != 0;
        }
    }
}
