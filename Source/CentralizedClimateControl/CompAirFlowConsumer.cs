using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl;

public class CompAirFlowConsumer : CompAirFlow
{
    public const string AirFlowOutputKey = "CentralizedClimateControl.AirFlowOutput";
    private const string IntakeTempKey = "CentralizedClimateControl.Consumer.ConvertedTemperature";
    private const string FlowEfficiencyKey = "CentralizedClimateControl.Consumer.FlowEfficiencyKey";
    private const string ThermalEfficiencyKey = "CentralizedClimateControl.Consumer.ThermalEfficiencyKey";
    private const string DisconnectedKey = "CentralizedClimateControl.Consumer.Disconnected";
    private const string ClosedKey = "CentralizedClimateControl.Consumer.Closed";
    public AirTypePriority AirTypePriority = AirTypePriority.Auto;

    private bool alertChange;

    public float ConvertedTemperature;
    private CompFlickable flickableComp;

    public float ExhaustAirFlow => Props.baseAirExhaust;

    public float FlowEfficiency => AirFlowNet.FlowEfficiency;

    public float ThermalEfficiency => AirFlowNet.ThermalEfficiency;

    /// <summary>
    ///     Post Spawn for Component
    /// </summary>
    /// <param name="respawningAfterLoad">Unused Flag</param>
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterConsumer(this);
        flickableComp = parent.GetComp<CompFlickable>();

        base.PostSpawnSetup(respawningAfterLoad);
    }

    /// <summary>
    ///     Method called during Game Save/Load
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();

        Scribe_Values.Look(ref AirTypePriority, "airTypePriority", AirTypePriority.Auto);
        alertChange = true;
    }

    /// <summary>
    ///     Component De-spawned from Map
    /// </summary>
    /// <param name="map">RimWorld Map</param>
    /// <param name="mode"></param>
    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        CentralizedClimateControlUtility.GetNetManager(map).DeregisterConsumer(this);
        ResetFlowVariables();
        base.PostDeSpawn(map, mode);
    }

    /// <summary>
    ///     Extra Component Inspection string
    /// </summary>
    /// <returns>String Containing information for Consumers</returns>
    public override string CompInspectStringExtra()
    {
        if (!flickableComp.SwitchIsOn)
        {
            return ClosedKey.Translate() + "\n" + base.CompInspectStringExtra();
        }

        if (!IsOperating())
        {
            return base.CompInspectStringExtra();
        }

        if (!IsActive())
        {
            return (DisconnectedKey.Translate() + "\n" + base.CompInspectStringExtra()).Trim();
        }

        var str = IntakeTempKey.Translate($"{ConvertedTemperature.ToStringTemperature("F0")}\n");
        str += FlowEfficiencyKey.Translate($"{Mathf.FloorToInt(AirFlowNet.FlowEfficiency * 100)}%\n");
        str += ThermalEfficiencyKey.Translate($"{Mathf.FloorToInt(AirFlowNet.ThermalEfficiency * 100)}%\n" +
                                              base.CompInspectStringExtra());

        return str.Trim();
    }

    /// <summary>
    ///     Set the Pipe Priority for Consumers
    /// </summary>
    /// <param name="priority">Priority to Switch to.</param>
    public void SetPriority(AirTypePriority priority)
    {
        alertChange = true;
        AirTypePriority = priority;
        AirFlowNet = null;
    }

    /// <summary>
    ///     Tick for Consumers. Here:
    ///     - We Rebuild if Priority is Changed
    ///     - We take the Converted Temperature from Climate Units
    /// </summary>
    public void TickRare()
    {
        if (alertChange)
        {
            CentralizedClimateControlUtility.GetNetManager(parent.Map).IsDirty = true;
            alertChange = false;
        }

        if (!IsOperating())
        {
            return;
        }

        ConvertedTemperature = AirFlowNet.AverageConvertedTemperature;
    }

    public override bool IsOperating()
    {
        return flickableComp.SwitchIsOn && base.IsOperating();
    }

    /// <summary>
    ///     Reset the Flow Variables and Forward the Control to Base class for more reset.
    /// </summary>
    protected override void ResetFlowVariables()
    {
        ConvertedTemperature = 0.0f;
        base.ResetFlowVariables();
    }

    /// <summary>
    ///     Check if Consumer Can work.
    ///     This check is used after checking for Power.
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