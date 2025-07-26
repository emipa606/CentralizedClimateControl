using Verse;

namespace CentralizedClimateControl;

public class CompAirFlow : ThingComp
{
    private const string NotConnectedKey = "CentralizedClimateControl.AirFlowNetDisconnected";
    private const string ConnectedKey = "CentralizedClimateControl.AirFlowNetConnected";
    private const string AirTypeKey = "CentralizedClimateControl.AirType";
    private const string HotAirKey = "CentralizedClimateControl.HotAir";
    private const string ColdAirKey = "CentralizedClimateControl.ColdAir";
    private const string FrozenAirKey = "CentralizedClimateControl.FrozenAir";
    private const string TotalNetworkAirKey = "CentralizedClimateControl.TotalNetworkAir";
    private const string GridIdKey = "CentralizedClimateControl.DebugInfo.GridId";

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
    /// <param name="mode"></param>
    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        CentralizedClimateControlUtility.GetNetManager(map).DeregisterPipe(this);
        ResetFlowVariables();

        base.PostDeSpawn(map, mode);
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

        string inspectStringExtra = ConnectedKey.Translate();

        if (FlowType != AirFlowType.Any)
        {
            inspectStringExtra += "\n";
            inspectStringExtra += GetAirTypeString(FlowType);
        }

        inspectStringExtra += "\n";
        inspectStringExtra += TotalNetworkAirKey.Translate(AirFlowNet.CurrentIntakeAir);

        if (!DebugSettings.godMode)
        {
            return inspectStringExtra.Trim();
        }

        inspectStringExtra += "\n";
        inspectStringExtra += GetDebugString();

        return inspectStringExtra.Trim();
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

    protected string GetDebugString()
    {
        return GridIdKey.Translate(GridID);
    }

    /// <summary>
    ///     Get the Type of Air as String. Hot Cold Frozen etc.
    /// </summary>
    /// <param name="type">Enum for AirFlow Type</param>
    /// <returns>Translated String</returns>
    protected static string GetAirTypeString(AirFlowType type)
    {
        var airTypeString = "";
        switch (type)
        {
            case AirFlowType.Cold:
                airTypeString += AirTypeKey.Translate(ColdAirKey.Translate());
                break;

            case AirFlowType.Hot:
                airTypeString += AirTypeKey.Translate(HotAirKey.Translate());
                break;

            case AirFlowType.Frozen:
                airTypeString += AirTypeKey.Translate(FrozenAirKey.Translate());
                break;

            default:
                airTypeString += AirTypeKey.Translate("Unknown");
                break;
        }

        return airTypeString;
    }
}