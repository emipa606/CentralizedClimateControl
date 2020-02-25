using Verse;

namespace CentralizedClimateControl
{
    public enum AirFlowType
    {
        Hot = 0,
        Cold = 1,
        Frozen = 2,
        Any = 3
    }

    public enum AirTypePriority
    {
        Hot = 0,
        Cold = 1,
        Frozen = 2,
        Auto = 3
    }

    public class CompProperties_AirFlow : CompProperties
    {
        public bool transmitsAir;

        public float baseAirFlow;

        public AirFlowType flowType;

        public float baseAirExhaust;

        public float thermalCapacity;
    }
}
