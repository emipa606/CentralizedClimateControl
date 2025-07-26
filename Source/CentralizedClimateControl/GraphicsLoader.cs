using Verse;

namespace CentralizedClimateControl;

[StaticConstructorOnStartup]
public class GraphicsLoader
{
    // Actual Atlas
    private static readonly Graphic BlankPipeAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Blank_AirPipe_Atlas", ShaderDatabase.Transparent);

    private static readonly Graphic HotPipeAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Atlas", ShaderDatabase.Transparent);

    private static readonly Graphic ColdPipeAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Atlas", ShaderDatabase.Transparent);

    private static readonly Graphic FrozenPipeAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Atlas", ShaderDatabase.Transparent);

    // Overlays
    private static readonly Graphic HotPipeOverlayAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Overlay_Atlas",
            ShaderDatabase.MetaOverlay);

    private static readonly Graphic ColdPipeOverlayAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Overlay_Atlas",
            ShaderDatabase.MetaOverlay);

    private static readonly Graphic FrozenPipeOverlayAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Overlay_Atlas",
            ShaderDatabase.MetaOverlay);

    private static readonly Graphic AnyPipeOverlayAtlas =
        GraphicDatabase.Get<Graphic_Single>("Things/Building/Any_AirPipe_Overlay_Atlas",
            ShaderDatabase.MetaOverlay);

    public static readonly GraphicPipe GraphicHotPipe = new(HotPipeAtlas, AirFlowType.Hot);
    public static readonly GraphicPipe GraphicHotPipeHidden = new(BlankPipeAtlas, AirFlowType.Hot);
    public static readonly GraphicPipe GraphicColdPipe = new(ColdPipeAtlas, AirFlowType.Cold);
    public static readonly GraphicPipe GraphicColdPipeHidden = new(BlankPipeAtlas, AirFlowType.Cold);
    public static readonly GraphicPipe GraphicFrozenPipe = new(FrozenPipeAtlas, AirFlowType.Frozen);
    public static readonly GraphicPipe GraphicFrozenPipeHidden = new(BlankPipeAtlas, AirFlowType.Frozen);

    public static readonly GraphicPipe_Overlay GraphicHotPipeOverlay =
        new(HotPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Hot);

    public static readonly GraphicPipe_Overlay GraphicColdPipeOverlay =
        new(ColdPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Cold);

    public static readonly GraphicPipe_Overlay GraphicFrozenPipeOverlay =
        new(FrozenPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Frozen);
}