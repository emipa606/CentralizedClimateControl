using Verse;

namespace CentralizedClimateControl;

public class CompProperties_AirFlow : CompProperties
{
    public float baseAirExhaust;

    public float baseAirFlow;

    public AirFlowType flowType;

    public float thermalCapacity;
    public bool transmitsAir;
}