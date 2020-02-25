using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public static class CentralizedClimateControlUtility
    {
        private const string SwitchPipeAutoKey = "CentralizedClimateControl.Command.SwitchPipe.Auto";
        private const string SwitchPipeRedKey = "CentralizedClimateControl.Command.SwitchPipe.Red";
        private const string SwitchPipeBlueKey = "CentralizedClimateControl.Command.SwitchPipe.Blue";
        private const string SwitchPipeCyanKey = "CentralizedClimateControl.Command.SwitchPipe.Cyan";

        /// <summary>
        /// Get the Network Manager of the Map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <returns>AirFlow Net Manager</returns>
        public static AirFlowNetManager GetNetManager(Map map)
        {
            return map.GetComponent<AirFlowNetManager>();
        }

        /// <summary>
        /// Gizmo for Changing Pipes
        /// </summary>
        /// <param name="compAirFlowConsumer">Component Asking for Gizmo</param>
        /// <returns>Action Button Gizmo</returns>
        public static Command_Action GetPipeSwitchToggle(CompAirFlowConsumer compAirFlowConsumer)
        {
            var currentPriority = compAirFlowConsumer.AirTypePriority;
            Texture2D icon;
            string label;

            switch (currentPriority)
            {
                case AirTypePriority.Auto:
                    label = SwitchPipeAutoKey.Translate();
                    icon = ContentFinder<Texture2D>.Get("UI/PipeSelect_Auto");
                    break;

                case AirTypePriority.Hot:
                    label = SwitchPipeRedKey.Translate();
                    icon = ContentFinder<Texture2D>.Get("UI/PipeSelect_Red");
                    break;
                case AirTypePriority.Cold:
                    label = SwitchPipeBlueKey.Translate();
                    icon = ContentFinder<Texture2D>.Get("UI/PipeSelect_Blue");
                    break;
                case AirTypePriority.Frozen:
                    label = SwitchPipeCyanKey.Translate();
                    icon = ContentFinder<Texture2D>.Get("UI/PipeSelect_Cyan");
                    break;

                default:
                    label = SwitchPipeAutoKey.Translate();
                    icon = ContentFinder<Texture2D>.Get("UI/PipeSelect_Auto");
                    break;
            }

            return new Command_Action()
            {
                defaultLabel = label,
                defaultDesc = "CentralizedClimateControl.Command.SwitchPipe.Desc".Translate(),
                hotKey = KeyBindingDefOf.Misc4,
                icon = icon,
                action = delegate
                {
                    switch (currentPriority)
                    {
                        case AirTypePriority.Auto:
                            compAirFlowConsumer.SetPriority(AirTypePriority.Hot);
                            break;

                        case AirTypePriority.Hot:
                            compAirFlowConsumer.SetPriority(AirTypePriority.Cold);
                            break;

                        case AirTypePriority.Cold:
                            compAirFlowConsumer.SetPriority(AirTypePriority.Frozen);
                            break;
                        case AirTypePriority.Frozen:
                            compAirFlowConsumer.SetPriority(AirTypePriority.Auto);
                            break;

                        default:
                            compAirFlowConsumer.SetPriority(AirTypePriority.Auto);
                            break;
                    }
                }
            };
        }
    }
}
