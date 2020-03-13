using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public class Building_HotAirPipe: Building_AirPipe
    {
        public override Graphic Graphic
        {
            get
            {
                if(def.defName == "redAirPipeHidden") return GraphicsLoader.GraphicHotPipeHidden;
                return GraphicsLoader.GraphicHotPipe;
            }
        }
    }
   
}
