using Verse;

namespace CentralizedClimateControl;

public class Building_AirPipe : Building
{
    public CompAirFlowPipe CompAirFlowPipe;
    public AirFlowType FlowType;

    /// <summary>
    ///     Building spawned on the map
    /// </summary>
    /// <param name="map">RimWorld Map</param>
    /// <param name="respawningAfterLoad">Unused flag</param>
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        CompAirFlowPipe = GetComp<CompAirFlowPipe>();
    }
}