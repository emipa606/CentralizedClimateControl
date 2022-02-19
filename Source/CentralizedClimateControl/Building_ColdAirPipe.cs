using Verse;

namespace CentralizedClimateControl;

public class Building_ColdAirPipe : Building_AirPipe
{
    public override Graphic Graphic
    {
        get
        {
            if (def.defName == "blueAirPipeHidden")
            {
                return GraphicsLoader.GraphicColdPipeHidden;
            }

            return GraphicsLoader.GraphicColdPipe;
        }
    }
}