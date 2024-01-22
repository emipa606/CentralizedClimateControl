﻿using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl;

public class CompAirFlowTempControl : CompAirFlow
{
    public const string TemperatureArrowKey = "CentralizedClimateControl.Producer.TemperatureArrow";
    public const string TargetTemperatureKey = "CentralizedClimateControl.Producer.TargetTemperature";
    public const string ExhaustBlockedKey = "CentralizedClimateControl.Producer.ExhaustBlocked";

    private const float DeltaSmooth = 96.0f;
    public float ConvertedTemperature;
    public float DeltaTemperature;

    protected CompFlickable FlickableComp;

    public float IntakeTemperature;
    public bool IsBlocked;

    public bool IsBrokenDown;
    public bool IsHeating;

    public bool IsOperatingAtHighPower;
    public bool IsPoweredOff;
    public bool IsStable;
    public float TargetTemperature = 21.0f;

    public float ThermalCapacity => Props.thermalCapacity;

    /// <summary>
    ///     Debug String for an Air Flow Climate Control
    ///     Shows info about Air Flow etc.
    /// </summary>
    public string DebugString
    {
        get
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{parent.LabelCap} CompAirFlow:");
            stringBuilder.AppendLine($"   AirFlow IsOperating: {IsOperating()}");
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    ///     Post Spawn for Component
    /// </summary>
    /// <param name="respawningAfterLoad">Unused Flag</param>
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterTempControl(this);
        FlickableComp = parent.GetComp<CompFlickable>();

        base.PostSpawnSetup(respawningAfterLoad);
    }

    /// <summary>
    ///     Despawn Event for an Air Climate Control Component
    /// </summary>
    /// <param name="map">RimWorld Map</param>
    public override void PostDeSpawn(Map map)
    {
        CentralizedClimateControlUtility.GetNetManager(map).DeregisterTempControl(this);
        ResetFlowVariables();
        base.PostDeSpawn(map);
    }

    /// <summary>
    ///     Game Save/Load Event. Here we save or restore the temperature changes in the network.
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref DeltaTemperature, "deltaTemperature");
        Scribe_Values.Look(ref IntakeTemperature, "intakeTemperature");
        Scribe_Values.Look(ref ConvertedTemperature, "convertedTemperature");
    }

    /// <summary>
    ///     Extra Component Inspection string
    /// </summary>
    /// <returns>String Containing information for Climate Control</returns>
    public override string CompInspectStringExtra()
    {
        var inspectStringExtra = "";

        if (IsPoweredOff || IsBrokenDown)
        {
            return null;
        }

        if (IsBlocked)
        {
            inspectStringExtra += ExhaustBlockedKey.Translate();
            return inspectStringExtra;
        }

        if (IsOperating())
        {
            //var intake = IntakeTemperature.ToStringTemperature("F0");
            //var converted = ConvertedTemperature.ToStringTemperature("F0");
            //str += TemperatureArrowKey.Translate(intake, converted);
            inspectStringExtra += TemperatureArrowKey.Translate(
                IntakeTemperature.ToStringTemperature("F0"),
                ConvertedTemperature.ToStringTemperature("F0")
            );
        }

        if (!DebugSettings.godMode)
        {
            return inspectStringExtra;
        }

        inspectStringExtra += "\n";
        inspectStringExtra += GetDebugString();

        return inspectStringExtra;
    }

    /// <summary>
    ///     Reset the Flow Variables for Producers and Forward the Control to Base class for more reset.
    /// </summary>
    protected override void ResetFlowVariables()
    {
        DeltaTemperature = 0.0f;
        TargetTemperature = 21.0f;
        ConvertedTemperature = 0.0f;
        IntakeTemperature = 0.0f;
        IsOperatingAtHighPower = false;
        IsBlocked = false;
        IsBrokenDown = false;
        IsPoweredOff = false;

        base.ResetFlowVariables();
    }

    /// <summary>
    ///     Tick for Climate Control
    ///     Here we calculate the growth of Delta Temperature which is increased or decrased based on Intake and Target
    ///     Temperature.
    /// </summary>
    /// <param name="compTempControl">Current Temperature Control Component of the Building</param>
    public void TickRare(CompTempControl compTempControl)
    {
        IntakeTemperature = AirFlowNet.AverageIntakeTemperature;
        TargetTemperature = compTempControl.targetTemperature;
        ConvertedTemperature = IntakeTemperature + DeltaTemperature;

        GenerateDelta(compTempControl);
    }

    /// <summary>
    ///     Check if Temperature Control is active or not. Needs Consumers and shouldn't be Blocked
    /// </summary>
    /// <returns>Boolean Active State</returns>
    public bool IsActive()
    {
        if (IsPoweredOff || IsBrokenDown)
        {
            return false;
        }

        return AirFlowNet.Producers.Count != 0 && !IsBlocked;
    }

    /// <summary>
    ///     Calculate the Temperature Delta for the Tick.
    /// </summary>
    /// <param name="compTempControl">Temperature Control Component</param>
    private void GenerateDelta(CompTempControl compTempControl)
    {
        var targetDelta = TargetTemperature - IntakeTemperature;
        var currentDelta = ConvertedTemperature - IntakeTemperature;

        IsHeating = TargetTemperature > IntakeTemperature;

        if (Mathf.Abs(targetDelta - currentDelta) < 1.0f)
        {
            DeltaTemperature += targetDelta - currentDelta;
            IsStable = true;
            return;
        }

        IsStable = false;
        //var deltaDelta = targetDelta - currentDelta;

        //var deltaSmoothened = deltaDelta / DeltaSmooth;
        //var deltaSmoothened = (targetDelta - currentDelta) / DeltaSmooth;
        //DeltaTemperature += (compTempControl.Props.energyPerSecond * AirFlowNet.ThermalEfficiency) * deltaSmoothened;
        DeltaTemperature += compTempControl.Props.energyPerSecond
                            * AirFlowNet.ThermalEfficiency
                            * ((targetDelta - currentDelta) / DeltaSmooth);
    }
}