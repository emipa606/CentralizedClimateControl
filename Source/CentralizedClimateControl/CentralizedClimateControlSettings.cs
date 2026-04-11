using Verse;

namespace CentralizedClimateControl;

public class CentralizedClimateControlSettings : ModSettings
{
    public float AirExhaustMultiplier = 1f;
    public float AirFlowMultiplier = 1f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref AirFlowMultiplier, "airFlowMultiplier", 1f);
        Scribe_Values.Look(ref AirExhaustMultiplier, "airExhaustMultiplier", 1f);
    }
}