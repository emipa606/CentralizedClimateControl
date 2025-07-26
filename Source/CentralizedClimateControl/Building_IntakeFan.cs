using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl;

public class Building_IntakeFan : Building_AirFlowControl
{
    private const float EfficiencyLossPerWindCubeBlocked = 0.0076923077f;
    private readonly int _windCellsBlocked = 0;

    private CompAirFlowProducer CompAirProducer;

    /// <summary>
    ///     Building spawned on the map
    /// </summary>
    /// <param name="map">RimWorld Map</param>
    /// <param name="respawningAfterLoad">Unused flag</param>
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        CompAirProducer = GetComp<CompAirFlowProducer>();
        CompAirProducer.Props.flowType = AirFlowType.Any;
    }

    /// <summary>
    ///     Tick Intake Fan. Check the surrondings and generate Air Flow if all clear.
    /// </summary>
    public override void TickRare()
    {
        if (!CompPowerTrader.PowerOn)
        {
            CompAirProducer.IsPoweredOff = true;
            CompAirProducer.CurrentAirFlow = 0;
            return;
        }

        CompAirProducer.IsPoweredOff = false;
        CompAirProducer.IsBrokenDown = this.IsBrokenDown();

        var sumTemp = 0f;
        var list = GenAdj.CellsAdjacent8Way(Position, Rotation, def.Size).ToList();

        var totalVaccum = 0f;

        foreach (var intVec in list)
        {
            if (intVec.Impassable(Map))
            {
                CompAirProducer.CurrentAirFlow = 0;
                CompAirProducer.IsBlocked = true;
                return;
            }

            sumTemp += intVec.GetTemperature(Map);
            totalVaccum += intVec.GetVacuum(Map);
        }

        CompAirProducer.IsBlocked = false;

        if (!CompAirProducer.IsActive())
        {
            return;
        }

        CompAirProducer.IntakeTemperature = sumTemp / list.Count;
        CompAirProducer.CurrentAirFlow = CompAirProducer.Props.baseAirFlow -
                                         (_windCellsBlocked * EfficiencyLossPerWindCubeBlocked);

        if (totalVaccum > 0)
        {
            CompAirProducer.CurrentAirFlow *= 1 - (totalVaccum / list.Count);
        }
    }
}