using Verse;

namespace CentralizedClimateControl;

public class Building_FrozenAirPipe : Building_AirPipe
{
    public override Graphic Graphic => def.defName == "cyanAirPipeHidden"
        ? GraphicsLoader.GraphicFrozenPipeHidden
        : GraphicsLoader.GraphicFrozenPipe;
}