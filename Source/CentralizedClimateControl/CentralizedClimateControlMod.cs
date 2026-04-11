using Mlie;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl;

[StaticConstructorOnStartup]
public class CentralizedClimateControlMod : Mod
{
    public static CentralizedClimateControlSettings Settings;
    private static string currentVersion;

    public CentralizedClimateControlMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<CentralizedClimateControlSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "Centralized Climate Control";
    }

    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing = new Listing_Standard();
        listing.Begin(rect);
        listing.Gap();
        Settings.AirFlowMultiplier = Widgets.HorizontalSlider(
            listing.GetRect(24f),
            Settings.AirFlowMultiplier,
            0f,
            10f,
            true,
            "CentralizedClimateControl.AirFlowMultiplier".Translate(Settings.AirFlowMultiplier.ToStringPercent()),
            "0%",
            "1000%");
        listing.Gap();
        Settings.AirExhaustMultiplier = Widgets.HorizontalSlider(
            listing.GetRect(24f),
            Settings.AirExhaustMultiplier,
            0f,
            10f,
            true,
            "CentralizedClimateControl.AirExhaustMultiplier".Translate(Settings.AirExhaustMultiplier.ToStringPercent()),
            "0%",
            "1000%");
        if (currentVersion != null)
        {
            listing.Gap();
            GUI.contentColor = Color.gray;
            listing.Label("CentralizedClimateControl.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing.End();
    }
}