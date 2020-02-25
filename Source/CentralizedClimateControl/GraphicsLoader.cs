using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    public class GraphicsLoader
    {
        // Actual Atlas
        public static Graphic HotPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Atlas", ShaderDatabase.Transparent);
        public static Graphic ColdPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Atlas", ShaderDatabase.Transparent);
        public static Graphic FrozenPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Atlas", ShaderDatabase.Transparent);

        // Overlays
        public static Graphic HotPipeOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
        public static Graphic ColdPipeOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
        public static Graphic FrozenPipeOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
        public static Graphic AnyPipeOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Any_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);

        public static GraphicPipe GraphicHotPipe = new GraphicPipe(GraphicsLoader.HotPipeAtlas, AirFlowType.Hot);
        public static GraphicPipe GraphicColdPipe = new GraphicPipe(GraphicsLoader.ColdPipeAtlas, AirFlowType.Cold);
        public static GraphicPipe GraphicFrozenPipe = new GraphicPipe(GraphicsLoader.FrozenPipeAtlas, AirFlowType.Frozen);

        public static GraphicPipe_Overlay GraphicHotPipeOverlay = new GraphicPipe_Overlay(HotPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Hot);
        public static GraphicPipe_Overlay GraphicColdPipeOverlay = new GraphicPipe_Overlay(ColdPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Cold);
        public static GraphicPipe_Overlay GraphicFrozenPipeOverlay = new GraphicPipe_Overlay(FrozenPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Frozen);
    }
}
