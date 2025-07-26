using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl;

public class AirFlowNetManager : MapComponent
{
    private const int RebuildValue = -2;

    private readonly List<AirFlowNet> backupNets = [];
    private readonly List<CompAirFlowConsumer> cachedConsumers = [];
    private readonly List<CompAirFlow> cachedPipes = [];
    private readonly List<CompAirFlowProducer> cachedProducers = [];
    private readonly List<CompAirFlowTempControl> cachedTempControls = [];
    private readonly bool[] dirtyPipeFlag;
    private readonly int pipeCount;

    private readonly int[,] pipeGrid;
    private List<AirFlowNet> cachedNets = [];
    public bool IsDirty;
    private int masterId;

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
        pipeGrid = new int[length, map.AllCells.Count()];

        pipeCount = length;

        dirtyPipeFlag = new bool[length];
        for (var i = 0; i < dirtyPipeFlag.Length; i++)
        {
            dirtyPipeFlag[i] = true;

            for (var j = 0; j < pipeGrid.GetLength(1); j++)
            {
                pipeGrid[i, j] = RebuildValue;
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
        if (!cachedPipes.Contains(pipe))
        {
            cachedPipes.Add(pipe);
            cachedPipes.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (cachedPipes.Contains(pipe))
        {
            cachedPipes.Remove(pipe);
            cachedPipes.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (!cachedTempControls.Contains(device))
        {
            cachedTempControls.Add(device);
            cachedTempControls.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (cachedTempControls.Contains(device))
        {
            cachedTempControls.Remove(device);
            cachedTempControls.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (!cachedProducers.Contains(pipe))
        {
            cachedProducers.Add(pipe);
            cachedProducers.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (cachedProducers.Contains(pipe))
        {
            cachedProducers.Remove(pipe);
            cachedProducers.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (!cachedConsumers.Contains(device))
        {
            cachedConsumers.Add(device);
            cachedConsumers.Shuffle(); // ! Why Shuffle?  --Brain
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
        if (cachedConsumers.Contains(device))
        {
            cachedConsumers.Remove(device);
            cachedConsumers.Shuffle(); // ! Why Shuffle?  --Brain
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
        return pipeGrid[(int)flowType, map.cellIndices.CellToIndex(pos)] != RebuildValue;
    }

    /// <summary>
    ///     Update Map Event
    ///     - Check if Dirty.
    ///     - If Dirty then Reconstruct Pipe Grids
    ///     - Reset Dirty Flags and Update the Cached Variables storing info on the Networks
    /// </summary>
    public override void MapComponentUpdate()
    {
        base.MapComponentUpdate();

        if (!IsDirty)
        {
            return;
        }

        foreach (var compAirFlow in cachedPipes)
        {
            compAirFlow.GridID = RebuildValue;
        }

        backupNets.Clear();

        for (var i = 0; i < pipeCount; i++)
        {
            if ((AirFlowType)i == AirFlowType.Any)
            {
                continue;
            }

            rebuildPipeGrid(i);
        }

        cachedNets = backupNets;

        // TODO: Not Optimized
        map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Buildings);
        map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Things);

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

        foreach (var airFlowNet in cachedNets)
        {
            airFlowNet.AirFlowNetTick();
        }

        base.MapComponentTick();
    }

    private void buildGridOfFlow(IntVec3 startCell, int gridId, int flowIndex, AirFlowNet network)
    {
        var visitedCells = new BitArray(pipeGrid.GetLength(1));
        var visitedLargeBuildings = new HashSet<Building>(ReferenceEqualityComparer.Instance);
        var toVisitQueue = new Queue<IntVec3>();

        visitedCells[map.cellIndices.CellToIndex(startCell)] = true;
        toVisitQueue.Enqueue(startCell);

        while (toVisitQueue.Count > 0)
        {
            var toVisitPos = toVisitQueue.Dequeue();

            foreach (var building in toVisitPos.GetThingList(map).OfType<Building>())
            {
                //if this is large building and alreay visited
                if (building.OccupiedRect().Area != 1 && !visitedLargeBuildings.Add(building))
                {
                    continue;
                }

                var any = false;
                foreach (var buildingAirComp in building.GetComps<CompAirFlow>()
                             .Where(item =>
                                 item.FlowType == (AirFlowType)flowIndex ||
                                 item.FlowType == AirFlowType.Any && item.GridID == RebuildValue))
                {
                    if (!validateBuildingPriority(buildingAirComp, network))
                    {
                        continue;
                    }

                    validateBuilding(buildingAirComp, network);

                    any = true;
                    buildingAirComp.GridID = gridId;
                }

                if (!any)
                {
                    continue;
                }

                foreach (var intVec in building.OccupiedRect())
                {
                    pipeGrid[flowIndex, map.cellIndices.CellToIndex(intVec)] = gridId;

                    //we assume buildings are small so this is better than iter edge(which contains an gc allocation)
                    enqueueNeighborCells(intVec, visitedCells, toVisitQueue);
                }
            }
        }
    }

    private void enqueueNeighborCells(IntVec3 pos, BitArray visitedCells, Queue<IntVec3> visitQueue)
    {
        for (var i = 0; i < 4; i++)
        {
            var nPos = pos + GenAdj.CardinalDirections[i];
            if (!nPos.InBounds(map))
            {
                continue;
            }

            var nIndex = map.cellIndices.CellToIndex(nPos);
            if (visitedCells[nIndex])
            {
                continue;
            }

            visitedCells[nIndex] = true;
            visitQueue.Enqueue(nPos);
        }
    }

    /// <summary>
    ///     Iterate on all the Occupied cells of a Cell. Here we can each Parent Occupied Rect cell.
    /// </summary>
    /// <param name="compAirFlow">The Object under scan</param>
    /// <param name="gridId">Grid ID of the current Network</param>
    /// <param name="flowIndex">Type of Air Flow</param>
    /// <param name="network">The Air Flow Network Object</param>
    private void parseParentCell(CompAirFlow compAirFlow, int gridId, int flowIndex, AirFlowNet network)
    {
        foreach (var current in compAirFlow.parent.OccupiedRect().EdgeCells)
        {
            scanCell(current, gridId, flowIndex, network);
        }
    }

    /// <summary>
    ///     Here we check for neighbouring Buildings and Pipes at `pos` param.
    ///     If we find the same Flow Type pipe or a Building (which hasn't been selected yet), then we add them to the list and
    ///     assign the same GridID.
    /// </summary>
    /// <param name="pos">Position of Cell to scan</param>
    /// <param name="gridId">Grid ID of the current Network</param>
    /// <param name="flowIndex">Type of Air Flow</param>
    /// <param name="network">The Air Flow Network Object</param>
    private void scanCell(IntVec3 pos, int gridId, int flowIndex, AirFlowNet network)
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
                    if (!validateBuildingPriority(buildingAirComp, network))
                    {
                        continue;
                    }

                    validateBuilding(buildingAirComp, network);
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
                    pipeGrid[flowIndex, map.cellIndices.CellToIndex(item)] = gridId;
                }

                compAirFlow.GridID = gridId;
                parseParentCell(compAirFlow, gridId, flowIndex, network);
            }
        }
    }

    /// <summary>
    ///     Main rebuild function. We Rebuild all different pipetypes here.
    /// </summary>
    /// <param name="flowIndex">Type of Pipe (Red, Blue, Cyan)</param>
    private void rebuildPipeGrid(int flowIndex)
    {
        var flowType = (AirFlowType)flowIndex;

        var runtimeNets = new List<AirFlowNet>();

        for (var i = 0; i < pipeGrid.GetLength(1); i++)
        {
            pipeGrid[flowIndex, i] = RebuildValue;
        }

        var pipes = cachedPipes.Where(item => item.FlowType == flowType).ToList();

        var listCopy = new List<CompAirFlow>(pipes);

        for (var compAirFlow = listCopy.FirstOrDefault();
             compAirFlow != null;
             compAirFlow = listCopy.FirstOrDefault())
        {
            var network = new AirFlowNet
            {
                GridID = masterId,
                FlowType = flowType
            };
            //network.GridID = compAirFlow.GridID;
            //network.FlowType = flowType;

            buildGridOfFlow(compAirFlow.parent.Position, masterId, flowIndex, network);
            listCopy.RemoveAll(item => item.GridID != RebuildValue);
            masterId++;

            network.AirFlowNetTick();
            runtimeNets.Add(network);
        }

        dirtyPipeFlag[flowIndex] = false;
        backupNets.AddRange(runtimeNets);
    }

    /// <summary>
    ///     Validate a Building. Check if it is a Consumer, Producer or Climate Control. If so, Add it to the network.
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    private static void validateBuilding(CompAirFlow compAirFlow, AirFlowNet network)
    {
        validateAsProducer(compAirFlow, network);
        validateAsTempControl(compAirFlow, network);
        validateAsConsumer(compAirFlow, network);
    }

    /// <summary>
    ///     Validate as an Air Flow Consumer
    /// </summary>
    /// <param name="compAirFlow">Building Component</param>
    /// <param name="network">Current Network</param>
    private static void validateAsConsumer(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (compAirFlow is not CompAirFlowConsumer consumer)
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
    private static bool validateBuildingPriority(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (compAirFlow == null)
        {
            return false;
        }

        if (compAirFlow is not CompAirFlowConsumer consumer)
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
    private static void validateAsProducer(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (compAirFlow is not CompAirFlowProducer producer)
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
    private static void validateAsTempControl(CompAirFlow compAirFlow, AirFlowNet network)
    {
        if (compAirFlow is not CompAirFlowTempControl tempControl)
        {
            return;
        }

        if (!network.TempControls.Contains(tempControl))
        {
            network.TempControls.Add(tempControl);
        }

        tempControl.AirFlowNet = network;
    }
}