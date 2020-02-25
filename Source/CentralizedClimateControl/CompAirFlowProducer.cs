using System.Text;
using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    public class CompAirFlowProducer : CompAirFlow
    {
        public const string AirFlowOutputKey = "CentralizedClimateControl.AirFlowOutput";
        public const string IntakeTempKey = "CentralizedClimateControl.Producer.IntakeTemperature";
        public const string IntakeBlockedKey = "CentralizedClimateControl.Producer.IntakeBlocked";

        [Unsaved]
        public bool IsOperatingAtHighPower;
        public bool IsBlocked = false;
        public bool IsBrokenDown = false;
        public bool IsPoweredOff = false;

        public float CurrentAirFlow;
        public float IntakeTemperature;
        protected CompFlickable FlickableComp;

        public float AirFlowOutput
        {
            get
            {
                if (IsOperating())
                {
                    return CurrentAirFlow;
                }

                return 0.0f;
            }
        }

        /// <summary>
        /// Debug String for a Air Flow Producer
        /// Shows info about Air Flow etc.
        /// </summary>
        public string DebugString
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(parent.LabelCap + " CompAirFlow:");
                stringBuilder.AppendLine("   AirFlow IsOperating: " + IsOperating());
                stringBuilder.AppendLine("   AirFlow Output: " + AirFlowOutput);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Post Spawn for Component
        /// </summary>
        /// <param name="respawningAfterLoad">Unused Flag</param>
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterProducer(this);
            FlickableComp = parent.GetComp<CompFlickable>();

            base.PostSpawnSetup(respawningAfterLoad);
        }

        /// <summary>
        /// Despawn Event for a Producer Component
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        public override void PostDeSpawn(Map map)
        {
            CentralizedClimateControlUtility.GetNetManager(map).DeregisterProducer(this);
            ResetFlowVariables();
            base.PostDeSpawn(map);
        }

        /// <summary>
        /// Extra Component Inspection string
        /// </summary>
        /// <returns>String Containing information for Producers</returns>
        public override string CompInspectStringExtra()
        {
            string str = "";

            if (IsPoweredOff || IsBrokenDown)
            {
                return null;
            }

            if (IsBlocked)
            {
                str += IntakeBlockedKey.Translate();
                return str;
            }

            if (IsOperating())
            {
                var convertedTemp = IntakeTemperature.ToStringTemperature("F0");
                str += AirFlowOutputKey.Translate(AirFlowOutput.ToString("#####0"));
                str += "\n";

                str += IntakeTempKey.Translate(convertedTemp);
                str += "\n";
            }

            return str + base.CompInspectStringExtra();
        }

        /// <summary>
        /// Check if Temperature Control is active or not. Needs Consumers and shouldn't be Blocked
        /// </summary>
        /// <returns>Boolean Active State</returns>
        public bool IsActive()
        {
            if (IsBlocked)
            {
                return false;
            }

            return !IsPoweredOff && !IsBrokenDown;
        }

        /// <summary>
        /// Reset the Flow Variables for Producers and Forward the Control to Base class for more reset.
        /// </summary>
        public override void ResetFlowVariables()
        {
            CurrentAirFlow = 0.0f;
            IntakeTemperature = 0.0f;
            IsOperatingAtHighPower = false;
            base.ResetFlowVariables();
        }
    }
}
