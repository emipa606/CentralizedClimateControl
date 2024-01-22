﻿using Verse;

namespace CentralizedClimateControl;

public class CompAirFlow : ThingComp
{
    public const string NotConnectedKey = "CentralizedClimateControl.AirFlowNetDisconnected";
    public const string ConnectedKey = "CentralizedClimateControl.AirFlowNetConnected";
    public const string AirTypeKey = "CentralizedClimateControl.AirType";
    public const string HotAirKey = "CentralizedClimateControl.HotAir";
    public const string ColdAirKey = "CentralizedClimateControl.ColdAir";
    public const string FrozenAirKey = "CentralizedClimateControl.FrozenAir";
    public const string TotalNetworkAirKey = "CentralizedClimateControl.TotalNetworkAir";
    public const string GridIdKey = "CentralizedClimateControl.DebugInfo.GridId";

    public AirFlowType FlowType => Props.flowType;

    public int GridID { get; set; } = -2;

    public AirFlowNet AirFlowNet { get; set; }

    public CompProperties_AirFlow Props => (CompProperties_AirFlow)props;

    /// <summary>
    ///     Reset the AirFlow Variables
    /// </summary>
    protected virtual void ResetFlowVariables()
    {
        AirFlowNet = null;
        GridID = -1;
    }

    /// <summary>
    ///     Component spawned on the map
    /// </summary>
    /// <param name="respawningAfterLoad">Unused flag</param>
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterPipe(this);
        base.PostSpawnSetup(respawningAfterLoad);
    }

    /// <summary>
    ///     Building de-spawned from the map
    /// </summary>
    /// <param name="map">RimWorld Map</param>
    public override void PostDeSpawn(Map map)
    {
        CentralizedClimateControlUtility.GetNetManager(map).DeregisterPipe(this);
        ResetFlowVariables();

        base.PostDeSpawn(map);
    }

    /// <summary>
    ///     Check if Air Flow Component is Working.
    ///     Must be connected to an AirFlow Network.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsOperating()
    {
        // No need for a local variable if it is returned right away. --Brain
        //bool isConnected = AirFlowNet != null;
        //return isConnected;

        return AirFlowNet != null;
    }

    /// <summary>
    ///     Inspect Component String
    /// </summary>
    /// <returns>String to be Displayed on the Component window</returns>
    public override string CompInspectStringExtra()
    {
        if (!IsOperating())
        {
            return NotConnectedKey.Translate();
        }

        string res = ConnectedKey.Translate();

        if (FlowType != AirFlowType.Any)
        {
            res += "\n";
            res += GetAirTypeString(FlowType);
        }

        res += "\n";
        res += TotalNetworkAirKey.Translate(AirFlowNet.CurrentIntakeAir);

        if (DebugSettings.godMode)
        {
            res += "\n";
            res += GetDebugString();
        }

        return res;
    }

    /// <summary>
    ///     Print the Component for Overlay Grid
    /// </summary>
    /// <param name="layer">Section Layer that is being Printed</param>
    /// <param name="type">AirFlow Type</param>
    public void PrintForGrid(SectionLayer layer, AirFlowType type)
    {
        switch (type)
        {
            case AirFlowType.Hot:
                GraphicsLoader.GraphicHotPipeOverlay.Print(layer, parent, 0);
                break;

            case AirFlowType.Cold:
                GraphicsLoader.GraphicColdPipeOverlay.Print(layer, parent, 0);
                break;

            case AirFlowType.Frozen:
                GraphicsLoader.GraphicFrozenPipeOverlay.Print(layer, parent, 0);
                break;

            case AirFlowType.Any:
                break;
        }
    }

    public string GetDebugString()
    {
        string res = GridIdKey.Translate(GridID);
        return res;
    }

    /// <summary>
    ///     Get the Type of Air as String. Hot Cold Frozen etc.
    /// </summary>
    /// <param name="type">Enum for AirFlow Type</param>
    /// <returns>Translated String</returns>
    protected string GetAirTypeString(AirFlowType type)
    {
        var res = "";
        switch (type)
        {
            case AirFlowType.Cold:
                res += AirTypeKey.Translate(ColdAirKey.Translate());
                break;

            case AirFlowType.Hot:
                res += AirTypeKey.Translate(HotAirKey.Translate());
                break;

            case AirFlowType.Frozen:
                res += AirTypeKey.Translate(FrozenAirKey.Translate());
                break;

            default:
                res += AirTypeKey.Translate("Unknown");
                break;
        }

        return res;
    }
}