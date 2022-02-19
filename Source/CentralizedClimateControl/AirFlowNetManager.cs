using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl;

public class AirFlowNetManager : MapComponent
{
    private const int RebuildValue = -2;

    private readonly List<AirFlowNet> _backupNets = new List<AirFlowNet>();
    private readonly int _pipeCount;
    private int _masterId;
    public List<CompAirFlowConsumer> CachedConsumers = new List<CompAirFlowConsumer>();
    public List<AirFlowNet> CachedNets = new List<AirFlowNet>();
    public List<CompAirFlow> CachedPipes = new List<CompAirFlow>();
    public List<CompAirFlowProducer> CachedProducers = new List<CompAirFlowProducer>();
    public List<CompAirFlowTempControl> CachedTempControls = new List<CompAirFlowTempControl>();
    public bool[] DirtyPipeFlag;
    public bool IsDirty;

    public int[,] PipeGrid;

    /// <summary>
    ///     Constructor of the Network Manager
    ///     - Init the Pipe Matrix
    ///     - Mark Dirty for 1st reconstruction
    /// </summary>
    /// <param name="map">RimWorld Map Object</param>
    public AirFlowNetManager(Map map) : base(map)
    {
        var length = Enum.GetValues(typeof(AirFlowType)).Length;
        //var num = map.AllCells.Count();
        //PipeGrid = new int[length, num];
        PipeGrid = new int[length, map.AllCells.Count()];

        _pipeCount = length;

        DirtyPipeFlag = new bool[length];
        for (var i = 0; i < DirtyPipeFlag.Length; i++)
        {
            DirtyPipeFlag[i] = true;

            for (var j = 0; j < PipeGrid.GetLength(1); j++)
            {
                PipeGrid[i, j] = RebuildValue;
            }
        }

        IsDirty = true;
    }

    /// <summary>
    ///     Register a Pipe to the Manager
    /// </summary>
    /// <param name="pipe">A Pipe's AirFlow Component</param>
    public void RegisterPipe(CompAirFlow pipe)
    {
        if (!CachedPipes.Contains(pipe))
        {
            CachedPipes.Add(pipe);
            CachedPipes.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Remove a Pipe from the Manager
    /// </summary>
    /// <param name="pipe">The Pipe's AirFlow Component</param>
    public void DeregisterPipe(CompAirFlow pipe)
    {
        if (CachedPipes.Contains(pipe))
        {
            CachedPipes.Remove(pipe);
            CachedPipes.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Register a Climate Control Device
    /// </summary>
    /// <param name="device">Climate Control Component</param>
    public void RegisterTempControl(CompAirFlowTempControl device)
    {
        if (!CachedTempControls.Contains(device))
        {
            CachedTempControls.Add(device);
            CachedTempControls.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Deregister a Climate Control Object from the Manager
    /// </summary>
    /// <param name="device">Climate Control Component</param>
    public void DeregisterTempControl(CompAirFlowTempControl device)
    {
        if (CachedTempControls.Contains(device))
        {
            CachedTempControls.Remove(device);
            CachedTempControls.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Register a Air Flow Producer
    /// </summary>
    /// <param name="pipe">Producer's Air Flow Component</param>
    public void RegisterProducer(CompAirFlowProducer pipe)
    {
        if (!CachedProducers.Contains(pipe))
        {
            CachedProducers.Add(pipe);
            CachedProducers.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Deregister a Producer from the Network Manager
    /// </summary>
    /// <param name="pipe">Producer's Component</param>
    public void DeregisterProducer(CompAirFlowProducer pipe)
    {
        if (CachedProducers.Contains(pipe))
        {
            CachedProducers.Remove(pipe);
            CachedProducers.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Register an Air Flow Consumer to the Network Manager
    /// </summary>
    /// <param name="device">Consumer's Air Flow Component</param>
    public void RegisterConsumer(CompAirFlowConsumer device)
    {
        if (!CachedConsumers.Contains(device))
        {
            CachedConsumers.Add(device);
            CachedConsumers.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    /// <summary>
    ///     Deregister a Consumer from the Network Manager
    /// </summary>
    /// <param name="device">Consumer's Air Flow Component</param>
    public void DeregisterConsumer(CompAirFlowConsumer device)
    {
        if (CachedConsumers.Contains(device))
        {
            CachedConsumers.Remove(device);
            CachedConsumers.Shuffle(); // ! Why Shuffle?  --Brain
        }

        // Useless function call  --Brain
        // DirtyPipeGrid();
        IsDirty = true;
    }

    // ? Why are those two methods even here? IsDirty is public, so why raise further calls? --Brain

    // <summary>
    // Dirty the flag for reconstruction
    // </summary>
    /* public void DirtyPipeGrid()
    {
        IsDirty = true;
    } */

    // <summary>
    // Dirty the flag for reconstruction
    // </summary>
    /* public void DirtyPipeWholeGrid()
    {
        IsDirty = true;
    } */

    /// <summary>
    ///     Check if that Zone in the Pipe Matrix has a Pipe of some sort or not.
    /// </summary>
    /// <param name="pos">Position of the cell</param>
    /// <param name="flowType">Airflow type</param>
    /// <returns>Boolean result if pipe exists at cell or not</returns>
    public bool ZoneAt(IntVec3 pos, AirFlowType flowType)
    {
        return PipeGrid[(int)flowType, map.cellIndices.CellToIndex(pos)] != RebuildValue;
    }

    /// <summary>
    ///     Same as ZoneAt but also checks for the GridID in the Pipe Matrix
    /// </summary>
    /// <param name="pos">Position of the cell</param>
    /// <param name="flowType">Airflow type</param>
    /// <param name="id">GridID to check for</param>
    /// <returns>Boolean result if perfect pipe exists at cell or not</returns>
    public bool PerfectMatch(IntVec3 pos, AirFlowType flowType, int id)
    {
        return PipeGrid[(int)flowType, map.cellIndices.CellToIndex(pos)] == id;
    }

    /// <summary>
    ///     Update Map Event
    ///     - Check if Dirty
    ///     - If it is Dirty then Reconstruct Pipe Grids
    ///     - Reset Dirty Flags and Update the Cached Variables storing info on the Networks
    /// </summary>
    public override void MapComponentUpdate()
    {
        base.MapComponentUpdate();

        if (!IsDirty)
        {
            return;
        }

        foreach (var compAirFlow in CachedPipes)
        {
            compAirFlow.GridID = RebuildValue;
        }

        _backupNets.Clear();

        for (var i = 0; i < _pipeCount; i++)
        {
            if ((AirFlowType)i == AirFlowType.Any)
            {
                continue;
            }

            RebuildPipeGrid(i);
        }

        CachedNets = _backupNets;

//             TODO: Not Optimized
        map.mapDrawer.WholeMapChanged(MapMeshFlag.Buildings);
        map.mapDrawer.WholeMapChanged(MapMeshFlag.Things);

        IsDirty = false;
    }

    /// <summary>
    ///     Tick of Map Component. Here we tick all the Air Networks that are built.
    /// </summary>
    public override void MapComponentTick()
    {
        if (IsDirty)
        {
            return;
        }

        foreach (var airFlowNet in CachedNets)
        {
            airFlowNet.AirFlowNetTick();
        }

        base.MapComponentTick();
    }

    /// <summary>
    ///     Iterate on all the Occupied cells of a Cell. Here we can each Parent Occupied Rect cell.
    /// </summary>
    /// <param name="compAirFlow">The Object under scan</param>
    /// <param name="gridId">Grid ID of the current Network</param>
    /// <param name="flowIndex">Type of Air Flow</param>
    /// <param name="network">The Air Flow Network Object</param>
    private void ParseParentCell(CompAirFlow compAirFlow, int gridId, int flowIndex, AirFlowNet network)
    {
        foreach (var current in compAirFlow.parent.OccupiedRect().EdgeCells)
        {
            ScanCell(current, gridId, flowIndex, network);
        }
    }

    /// <summary>
    ///     Here we check for neighbouring Buildings and Pipes at `pos` param.
    ///     If we find the same Flow Type pipe or a Building (which hasnt been selected yet), then we add them to the list and
    ///     assign the same GridID.
    /// </summary>
    /// <param name="pos">Position of Cell to scan</param>
    /// <param name="gridId">Grid ID of the current Network</param>
    /// <param name="flowIndex">Type of Air Flow</param>
    /// <param name="network">The Air Flow Network Object</param>
    public void ScanCell(IntVec3 pos, int gridId, int flowIndex, AirFlowNet network)
    {
        for (var i = 0; i < 4; i++)
        {
            //var thingList = (pos + GenAdj.CardinalDirections[i]).GetThingList(map);
            //var buildingList = thingList.OfType<Building>();
            //var buildingList = (pos + GenAdj.CardinalDirections[i]).GetThingList(map).OfType<Building>();

            var list = new List<CompAirFlow>();

            //foreach (var current in buildingList)
            foreach (var current in (pos + GenAdj.CardinalDirections[i]).GetThingList(map).OfType<Building>())
            {
                //var buildingAirComps = current.GetComps<CompAirFlow>().Where(item => item.FlowType == (AirFlowType)flowIndex || (item.FlowType == AirFlowType.Any && item.GridID == RebuildValue));

                //foreach (var buildingAirComp in buildingAirComps)
                foreach (var buildingAirComp in current.GetComps<CompAirFlow>()
                             .Where(item =>
                                 item.FlowType == (AirFlowType)flowIndex ||
                                 item.FlowType == AirFlowType.Any && item.GridID == RebuildValue))
                {
                    // var result = ValidateBuildingPriority(buildingAirComp, network);
                    // if(!result)
                    if (!ValidateBuildingPriority(buildingAirComp, network))
                    {
                        continue;
                    }

                    ValidateBuilding(buildingAirComp, network);
                    list.Add(buildingAirComp);
                }
            }

            if (!list.Any())
            {
                continue;
            }

            foreach (var compAirFlow in list)
            {
                if (compAirFlow.GridID != -2)
                {
                    continue;
                }

                //var iterator = compAirFlow.parent.OccupiedRect().GetIterator();
                //while (!iterator.Done())
                foreach (var item in compAirFlow.parent.OccupiedRect())
                {
                    PipeGrid[flowIndex, map.cellIndices.CellToIndex(item)] = gridId;
                }

                compAirFlow.GridID = gridId;
                ParseParentCell(compAirFlow, gridId, flowIndex, network);
            }
        }
    }

    /// <summary>
    ///     Main rebuild function. We Rebuild all different pipetypes here.
    /// </summary>
    /// <param name="flowIndex">Type of Pipe (Red, Blue, Cyan)</param>
    private void RebuildPipeGrid(int flowIndex)
    {
        var flowType = (AirFlowType)flowIndex;

        var runtimeNets = new List<AirFlowNet>();

        for (var i = 0; i < PipeGrid.GetLength(1); i++)
        {
            PipeGrid[flowIndex, i] = RebuildValue;
        }

        var cachedPipes = CachedPipes.Where(item => item.FlowType == flowType).ToList();

#if DEBUG
            Debug.Log("--- Start Rebuilding --- For Index: " + flowType);
            PrintPipes(cachedPipes);
#endif

        var listCopy = new List<CompAirFlow>(cachedPipes);

        for (var compAirFlow = listCopy.FirstOrDefault();
             compAirFlow != null;
             compAirFlow = listCopy.FirstOrDefault())
        {
            compAirFlow.GridID = _masterId;

            var network = new AirFlowNet
            {
                GridID = compAirFlow.GridID,
                FlowType = flowType
            };
            //network.GridID = compAirFlow.GridID;
            //network.FlowType = flowType;
            _masterId++;

            /* -------------------------------------------
             *
             * Scan the Position - Get all Buildings - And Assign to Network if Priority Allows
             *
             * -------------------------------------------
             */
            //var thingList = compAirFlow.parent.Position.GetThingList(map);
            //var buildingList = thingList.OfType<Building>();
            //var buildingList = compAirFlow.parent.Position.GetThingList(map).OfType<Building>();
            //foreach (var current in buildingList)
            foreach (var current in compAirFlow.parent.Position.GetThingList(map).OfType<Building>())
            {
                //var buildingAirComps = current.GetComps<CompAirFlow>().Where(item => item.FlowType == AirFlowType.Any && item.GridID == RebuildValue);

                //foreach (var buildingAirComp in buildingAirComps)
                foreach (var buildingAirComp in current.GetComps<CompAirFlow>()
                             .Where(item => item.FlowType == AirFlowType.Any && item.GridID == RebuildValue))
                {
                    //var result = ValidateBuildingPriority(buildingAirComp, network);
                    //if (!result)
                    if (!ValidateBuildingPriority(buildingAirComp, network))
                    {
                        continue;
                    }

                    ValidateBuilding(buildingAirComp, network);
                    //var itr = buildingAirComp.parent.OccupiedRect().GetIterator();
                    //while (!itr.Done())
                    foreach (var item in buildingAirComp.parent.OccupiedRect())
                    {
                        PipeGrid[flowIndex, map.cellIndices.CellToIndex(item)] = compAirFlow.GridID;
                    }

                    buildingAirComp.GridID = compAirFlow.GridID;
                }
            }

            /* -------------------------------------------
             *
             * Iterate the OccupiedRect of the Original compAirFlow (This is the Pipe)
             * So, We add the Pipe to the Grid.
             *
             * -------------------------------------------
             */
            //var iterator = compAirFlow.parent.OccupiedRect().GetIterator();
            //while (!iterator.Done())
            foreach (var item in compAirFlow.parent.OccupiedRect())
            {
                PipeGrid[flowIndex, map.cellIndices.CellToIndex(item)] = compAirFlow.GridID;
            }

            ParseParentCell(compAirFlow, compAirFlow.GridID, flowIndex, network);
            listCopy.RemoveAll(item => item.GridID != RebuildValue);

            network.AirFlowNetTick();
#if DEBUG
                Debug.Log(network.DebugString());
#endif
            runtimeNets.Add(network);
        }

        DirtyPipeFlag[flowIndex] = false;
#if DEBUG
            Debug.Log("--- Done Rebuilding ---");
#endif
        _backupNets.AddRange(runtimeNets);
    }

    /// <summary>
    ///     Validate a Building. Check if it is a Consumer, Producer or Climate Control. If so, Add it to the network.
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    private static void ValidateBuilding(CompAirFlow compAirFlow, AirFlowNet network)
    {
        ValidateAsProducer(compAirFlow, network);
        ValidateAsTempControl(compAirFlow, network);
        ValidateAsConsumer(compAirFlow, network);
    }

    /// <summary>
    ///     Validate as a Air Flow Consumer
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    private static void ValidateAsConsumer(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (!(compAirFlow is CompAirFlowConsumer consumer))
        {
            return;
        }

        if (!network.Consumers.Contains(consumer))
        {
            network.Consumers.Add(consumer);
        }

        consumer.AirFlowNet = network;
    }

    /// <summary>
    ///     Check Building Priority. If the Building is a Consumer, we can check for Priority.
    ///     If the Priority is Auto, then we skip the priority check
    ///     else we check if the Network air type matches the Priority. If it does match we add it to the network. Else we skip
    ///     it.
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    /// <returns>Result if we can add the Building to existing Network</returns>
    private static bool ValidateBuildingPriority(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (compAirFlow == null)
        {
            return false;
        }

        if (!(compAirFlow is CompAirFlowConsumer consumer))
        {
            return true;
        }

        var priority = consumer.AirTypePriority;

        if (priority == AirTypePriority.Auto)
        {
            return true;
        }

        return (int)priority == (int)network.FlowType;
    }

    /// <summary>
    ///     Validate Building as Air Flow Producer
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    private static void ValidateAsProducer(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (!(compAirFlow is CompAirFlowProducer producer))
        {
            return;
        }

        if (!network.Producers.Contains(producer))
        {
            network.Producers.Add(producer);
        }

        producer.AirFlowNet = network;
    }

    /// <summary>
    ///     Validate Building as Climate Control Building
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    private static void ValidateAsTempControl(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (!(compAirFlow is CompAirFlowTempControl tempControl))
        {
            return;
        }

        if (!network.TempControls.Contains(tempControl))
        {
            network.TempControls.Add(tempControl);
        }

        tempControl.AirFlowNet = network;
    }

    /// <summary>
    ///     Print the Pipes for Debug
    /// </summary>
    /// <param name="comps">Pipe List</param>
    private void PrintPipes(IEnumerable<CompAirFlow> comps)
    {
        var str = "\nPrinting CompAirFlows -";

        foreach (var compAirFlow in comps)
        {
            str += "\n  - " + compAirFlow.parent + " (GRID ID: " + compAirFlow.GridID + ") ";
        }

        Debug.Log(str);
    }
}