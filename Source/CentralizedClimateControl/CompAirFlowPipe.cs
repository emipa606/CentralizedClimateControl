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
        var inspectStringExtra = GetAirTypeString(Props.flowType);

        if (!DebugSettings.godMode)
        {
            return inspectStringExtra;
        }

        inspectStringExtra += "\n";
        inspectStringExtra += GetDebugString();

        return inspectStringExtra.Trim();
    }
}