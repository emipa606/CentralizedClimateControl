using Verse;

namespace CentralizedClimateControl;

public class Building_HotAirPipe : Building_AirPipe
{
    public override Graphic Graphic => def.defName == "redAirPipeHidden"
        ? GraphicsLoader.GraphicHotPipeHidden
        : GraphicsLoader.GraphicHotPipe;
}