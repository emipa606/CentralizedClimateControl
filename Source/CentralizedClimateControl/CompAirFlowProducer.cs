﻿using System.Text;
using RimWorld;
using Verse;

namespace CentralizedClimateControl;

public class CompAirFlowProducer : CompAirFlow
{
    public const string AirFlowOutputKey = "CentralizedClimateControl.AirFlowOutput";
    public const string IntakeTempKey = "CentralizedClimateControl.Producer.IntakeTemperature";
    public const string IntakeBlockedKey = "CentralizedClimateControl.Producer.IntakeBlocked";

    public float CurrentAirFlow;
    protected CompFlickable FlickableComp;
    public float IntakeTemperature;
    public bool IsBlocked = false;
    public bool IsBrokenDown = false;

    [Unsaved] public bool IsOperatingAtHighPower;

    public bool IsPoweredOff = false;

    public float AirFlowOutput => IsOperating() ? CurrentAirFlow : 0.0f;

    /// <summary>
    ///     Debug String for an Air Flow Producer
    ///     Shows info about Air Flow etc.
    /// </summary>
    public string DebugString
    {
        get
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{parent.LabelCap} CompAirFlow:");
            stringBuilder.AppendLine($"   AirFlow IsOperating: {IsOperating()}");
            stringBuilder.AppendLine($"   AirFlow Output: {AirFlowOutput}");
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    ///     Post Spawn for Component
    /// </summary>
    /// <param name="respawningAfterLoad">Unused Flag</param>
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterProducer(this);
        FlickableComp = parent.GetComp<CompFlickable>();

        base.PostSpawnSetup(respawningAfterLoad);
    }

    /// <summary>
    ///     Despawn Event for a Producer Component
    /// </summary>
    /// <param name="map">RimWorld Map</param>
    /// <param name="mode">Destroy Mode</param>
    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        CentralizedClimateControlUtility.GetNetManager(map).DeregisterProducer(this);
        ResetFlowVariables();
        base.PostDeSpawn(map, mode);
    }

    /// <summary>
    ///     Extra Component Inspection string
    /// </summary>
    /// <returns>String Containing information for Producers</returns>
    public override string CompInspectStringExtra()
    {
        var str = "";

        if (IsPoweredOff || IsBrokenDown)
        {
            return null;
        }

        if (IsBlocked)
        {
            str += IntakeBlockedKey.Translate();
            return str;
        }

        if (!IsOperating())
        {
            return str + base.CompInspectStringExtra();
        }

        //var convertedTemp = IntakeTemperature.ToStringTemperature("F0");
        str += AirFlowOutputKey.Translate(AirFlowOutput.ToString("#####0")) + "\n";
        //str += "\n";

        //str += IntakeTempKey.Translate(convertedTemp) + "\n";
        str += IntakeTempKey.Translate(IntakeTemperature.ToStringTemperature("F0")) + "\n";
        //str += "\n";

        return str + base.CompInspectStringExtra();
    }

    /// <summary>
    ///     Check if Temperature Control is active or not. Needs Consumers and shouldn't be Blocked
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
    ///     Reset the Flow Variables for Producers and Forward the Control to Base class for more reset.
    /// </summary>
    protected override void ResetFlowVariables()
    {
        CurrentAirFlow = 0.0f;
        IntakeTemperature = 0.0f;
        IsOperatingAtHighPower = false;
        base.ResetFlowVariables();
    }
}