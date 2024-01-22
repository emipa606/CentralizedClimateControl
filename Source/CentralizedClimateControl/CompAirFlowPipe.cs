using Verse;

namespace CentralizedClimateControl;

public class CompAirFlowPipe : CompAirFlow
{
    /// <summary>
    ///     Component Inspection for Pipes
    /// </summary>
    /// <returns>String to Display for Pipes</returns>
    public override string CompInspectStringExtra()
    {
        var res = GetAirTypeString(Props.flowType);

        if (DebugSettings.godMode)
        {
            res += "\n";
            res += GetDebugString();
        }

        return res;
    }
}