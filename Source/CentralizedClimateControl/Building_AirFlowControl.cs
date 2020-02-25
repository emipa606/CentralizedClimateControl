using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    public class Building_AirFlowControl : Building
    {
        public CompPowerTrader CompPowerTrader;

        /// <summary>
        /// Building spawned on the map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <param name="respawningAfterLoad">Unused flag</param>
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CompPowerTrader = GetComp<CompPowerTrader>();
        }
    }
}
