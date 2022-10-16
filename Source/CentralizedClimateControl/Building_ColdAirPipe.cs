using Verse;

namespace CentralizedClimateControl;

public class Building_ColdAirPipe : Building_AirPipe
{
    public override Graphic Graphic => def.defName == "blueAirPipeHidden"
        ? GraphicsLoader.GraphicColdPipeHidden
        : GraphicsLoader.GraphicColdPipe;
}