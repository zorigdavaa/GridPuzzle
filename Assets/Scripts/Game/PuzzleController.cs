using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using ZPackage;

public class PuzzleController : Mb
{
    Camera cam;
    Ray ray;
    int layermask;
    int roadMask;
    PuzzleSlot lastSlot;
    // Bot selectedObj;
    List<PuzzleSlot> chosenSlots;
    List<PuzzleSlot> Slots;
    public bool isPuzzling = false;
    public LineRenderer line;
    public Bot botPf;
    public Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        cam = FindObjectOfType<Camera>();
        layermask = LayerMask.GetMask("Bot");
        roadMask = LayerMask.GetMask("Road");

        GameManager.Instance.GamePlay += OnGamePlay;
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        StartPuzlle();
    }

    // bool dragging;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Refresh(5);
        }
        if (isPuzzling && IsDown)
        {
            ray = cam.ScreenPointToRay(MP);
            // bool isHit = Physics.Raycast(ray, out RaycastHit hit, 20, layermask);
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, 200);
            Bot selectedObj = null;
            if (isHit)
            {
                if (hit.transform.GetComponent<Bot>())
                {
                    selectedObj = hit.transform.GetComponent<Bot>();
                }
                else
                {
                    // print(hit.transform.gameObject.name);
                    Collider[] aroundObjs = Physics.OverlapSphere(hit.point, 0.5f);
                    // print(aroundObjs.Length);
                    float minDistance = Mathf.Infinity;
                    foreach (var obj in aroundObjs)
                    {
                        Bot bot = obj.transform.GetComponent<Bot>();
                        if (bot != null)
                        {
                            float distance = Vector3.Distance(hit.point, bot.transform.position);
                            if (distance < minDistance)
                            {
                                selectedObj = bot;
                                minDistance = distance;
                            }
                        }
                    }
                }
            }

            print("Down and hit was " + isHit);
            if (selectedObj && selectedObj.currentSlot?.GridNode)
            // if (hitObjs.Length > 0 && selectedObj && selectedObj.currentSlot?.GridNode)
            {
                selectedObj.Clicked();
                GridNode botNode = selectedObj.currentSlot.GridNode;
                // PuzzleSlot goSlot = chosenSlots.Where(x => x.GetBot() == null).First();
                List<PuzzleSlot> FreeGoNodes = chosenSlots.Where(x => x.GetBot() == null && !x.GetComponent<GridNode>().Blocked).ToList();
                GridNode goNode = null;
                if (FreeGoNodes.Count > 0)
                {
                    goNode = FreeGoNodes?.First().GridNode;
                }
                print(goNode + " go Nodes");
                print(!selectedObj.currentSlot.isChosenSlot + " 2nd");
                // if (goSlot && !selectedObj.currentSlot.isChosenSlot)
                if (goNode != null && !selectedObj.currentSlot.isChosenSlot)
                {
                    // List<PuzzleSlot> freeSlots = GetFreeSlots();
                    // List<Vector3> paths = FindPath(selectedObj.transform.position, goSlot.transform.position, freeSlots);
                    // List<Vector3> paths = FindPath(selectedObj.currentSlot.GridNode, goNode);
                    List<Vector3> paths = FindPath(botNode, goNode);
                    print("Path Count is  " + paths.Count);
                    // print(goNode.name + " Go Node");
                    if (paths.Count > 0)
                    {
                        print(paths.Count + " pathc");
                        selectedObj.currentSlot.SetBot(null);
                        Action afterAction = null;
                        // if (!chosenSlots.Any(x => x.GetBot() == null)) //when Full
                        selectedObj.TurnOnOutline(false);
                        // print("Free Go Nodes are " + FreeGoNodes.Count);
                        if (FreeGoNodes.Count <= 2) //when Last
                        {
                            var commonColorPair = CalcColors(out List<Bot> FrequentColorBots);
                            // print("Ijil ongotei humuus " + commonColorPair.Value);
                            if (commonColorPair.Value > 1) // 2 baiwal ochihdoo 3 bolno
                            {
                                // print("Jump Added " + commonColorPair.Value);
                                // afterAction = () => Jump();

                            }
                            List<PuzzleSlot> FreeSlots = Slots.Where(x => !x.isChosenSlot && x.GetBot() != null && !x.GetComponent<GridNode>().Blocked).ToList();
                            // if ((FreeGoNodes.Count == 1 && A.BusController.ManBusPos.HasFreeThreeSeat()) || FreeSlots.Count < 3)
                            if (FreeGoNodes.Count == 1)
                            {
                                print("Refesh added ");
                                print("1 condition " + (FreeGoNodes.Count == 1));
                                print("second - " + (FreeSlots.Count < 3) + " and Count is " + FreeSlots.Count);
                                afterAction += () => { Refresh(commonColorPair.Value); };
                                // Refresh(commonColorPair.Value);
                            }
                        }

                        selectedObj.GotoSlot(goNode, paths, afterAction);
                    }

                }

            }

            CheckAllBotPaths();
        }
    }

    GridNode checkCanGoChosenNode;
    //
    public void CheckAllBotPaths()
    {
        if (Slots != null)
        {
            // PuzzleSlot FreeGoNode = chosenSlots.Where(x => x.GetBot() == null).FirstOrDefault();
            // if (FreeGoNode)
            // {
            foreach (PuzzleSlot slot in Slots)
            {
                Bot bot = slot.GetBot();
                if (bot != null && !bot.currentSlot.isChosenSlot)
                {
                    GridNode botNode = bot.currentSlot.GridNode;
                    List<Vector3> paths = FindPath(botNode, checkCanGoChosenNode);
                    // print(paths.Count + " Paths");
                    if (paths.Count > 0)
                    {
                        // bot.ShowIsClickable(true);
                    }
                    // else
                    // {
                    //     //bot.ShowIsClickable(false);
                    // }
                }
            }
            // }
        }
    }


    private KeyValuePair<Color, int> CalcColors(out List<Bot> FrequentColorBots)
    {

        // chosenSlots = currentBusStop.Grid.GetChosenSlots();
        Dictionary<Color, int> ColorFreq = new Dictionary<Color, int>();
        foreach (var item in chosenSlots)
        {
            Bot bot = item.GetBot();
            if (bot)
            {
                Color color = bot.GetColor();
                if (ColorFreq.ContainsKey(color))
                {
                    ColorFreq[color]++;
                }
                else
                {
                    ColorFreq[color] = 1;
                }
            }

        }

        // print(ColorFreq.Count + "ijil ongo");
        // print(chosenSlots.Count + " songoson slot");
        var commonColorPair = ColorFreq.Where(x => x.Value == ColorFreq.Values.Max()).First();
        // List<Bot> FrequentColorBots = chosenSlots.Where(x=>x.GetBot().GetColor() == commonColorPair.Key).ToList();
        FrequentColorBots = chosenSlots.Select(x => x.GetBot()).Where(x => x?.GetColor() == commonColorPair.Key).ToList();
        // print("Ijil ongotei humuus " + FrequentColorBots.Count);
        // print("ColorFreq " + ColorFreq.Count);
        if (ColorFreq.Count > 1)
        {
            print("ColorFreq " + ColorFreq.Keys.ToList()[0]);
            print("ColorFreq " + ColorFreq.Keys.ToList()[1]);
        }

        return commonColorPair;
    }
    private KeyValuePair<Color, int> CalcColors(List<Bot> SeatchBots)
    {
        Dictionary<Color, int> ColorFreq = new Dictionary<Color, int>();
        foreach (var item in SeatchBots)
        {
            Color color = item.GetColor();
            if (ColorFreq.ContainsKey(color))
            {
                ColorFreq[color]++;
            }
            else
            {
                ColorFreq[color] = 1;
            }
        }

        var commonColorPair = ColorFreq.Where(x => x.Value == ColorFreq.Values.Max()).First();
        return commonColorPair;
    }


    public void Refresh(int commonColorCount)
    {
        List<PuzzleSlot> FreeSlots = Slots.Where(x => x.GetBot() != null && !x.isChosenSlot).ToList();
        List<PuzzleSlot> FreeGoNodes = chosenSlots.Where(x => x.GetBot() == null && !x.GetComponent<GridNode>().Blocked).ToList();
        // print(FreeSlots.Count + " Count was");
        // if (FreeGoNodes.Count == 1 || FreeSlots.Count < 3 || commonColorPair.Value < 3)
        // print("3 seats was " + A.BusController.ManBusPos.HasFreeThreeSeat());
        // if (!A.BusController.ManBusPos.HasFreeThreeSeat() && !isPuzzling)
        print("Coubnt was " + FreeSlots.Count);
        print("puzzling was  " + isPuzzling);
        print(FreeGoNodes.Count == 1);
        // if (isPuzzling && (FreeGoNodes.Count == 1 && !A.BusController.ManBusPos.HasFreeThreeSeat()) || FreeSlots.Count < 3)
        if (isPuzzling && (FreeGoNodes.Count < 1 || FreeSlots.Count == 0))
        // if (isPuzzling && FreeGoNodes.Count < 1)
        {
            print(FreeSlots.Count < 3);
            // print(commonColorPair.Key);
            // print(commonColorCount);
            print("Refershed  " + FreeGoNodes.Count);
            // HardRefresh();
            StartCoroutine(LocalCoroutine());
            IEnumerator LocalCoroutine()
            {
                // foreach (var item in Slots)
                // {
                //     if (item.GetBot() != null && item.GetBot().hiddenStatus.activeSelf)
                //     {
                //         item.GetBot().ShowIsClickable(true);
                //     }
                // }
                yield return new WaitForSeconds(1);
                HardRefresh();
            }
        }
    }

    public void HardRefresh()
    {
        // currentBusStop.Grid.RefillPuzzle(true);
        CheckAllBotPaths();
    }



    // private List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos, List<PuzzleSlot> freeSlots)
    public List<Vector3> FindPath(GridNode startPos, GridNode targetPos)
    {
        List<Vector3> path = new List<Vector3>();
        // Create lists for open and closed nodes
        List<GridNode> openList = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();

        // Add the start node to the open list
        openList.Add(startPos);

        // Start the A* algorithm
        while (openList.Count > 0)
        {
            // Get the node with the lowest F cost from the open list
            GridNode currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            // Remove the current node from the open list and add it to the closed set
            openList.Remove(currentNode);
            closedSet.Add(currentNode);

            // Check if we've reached the target node
            if (currentNode == targetPos)
            {
                // We've found the path, so retrace it and return
                // path = RetracePath(currentNode, targetPos);
                path = RetracePath(startPos, targetPos);
                // print("Found " + path.Count);
                return path;
            }

            // Get the neighboring nodes of the current node
            List<GridNode> neighbors = grid.GetNeighbors(currentNode);
            // Debug.Log(neighbors.Count + " Neighbors Count");
            // Process each neighboring node
            foreach (GridNode neighbor in neighbors)
            {
                // Skip this neighbor if it is not traversable or if it is in the closed set
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                // Calculate the new tentative G cost for this neighbor
                int newGCost = currentNode.GCost + grid.GetDistance(currentNode, neighbor);

                // If the new G cost is lower than the neighbor's current G cost or if the neighbor is not in the open list
                if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    // Update the neighbor's G cost and H cost
                    neighbor.GCost = newGCost;
                    neighbor.HCost = grid.GetDistance(neighbor, targetPos);

                    // Set the neighbor's parent to the current node
                    neighbor.Parent = currentNode;

                    // If the neighbor is not in the open list, add it
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        // print("Path not found");
        // No path found, return an empty path
        return path;
    }

    private List<Vector3> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        GridNode currentNode = endNode;
        path.Add(grid.GetWorldPosition(currentNode.X, currentNode.Y));
        while (currentNode != startNode)
        {
            // path.Add(currentNode.Position);
            path.Add(grid.GetWorldPosition(currentNode.X, currentNode.Y));
            // print(" Position was " + currentBusStop.Grid.GetWorldPosition(currentNode.X, currentNode.Y));
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    private int GetDistance(GridNode nodeA, GridNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.X - nodeB.X);
        int distanceY = Mathf.Abs(nodeA.Y - nodeB.Y);

        // Use Manhattan distance as the heuristic
        return distanceX + distanceY;
    }



    // private List<PuzzleSlot> GetFreeSlots()
    // {
    //     // List<Vector3> paths = new List<Vector3>();
    //     List<PuzzleSlot> FreeSlot = new List<PuzzleSlot>();
    //     List<PuzzleSlot> AllSlot = Slots;
    //     AllSlot.AddRange(chosenSlots);
    //     // Vector3 CurrentPos = selectedBot.currentSlot.transform.position;
    //     foreach (var item in AllSlot)
    //     {
    //         if (item.GetBot() == null)
    //         {
    //             FreeSlot.Add(item);
    //         }
    //     }
    //     print(FreeSlot.Count + " Free slots");
    //     return FreeSlot;
    // }


    internal void StartPuzlle()
    {
        // chosenSlots = stop.ChosenSlots;
        // List<PuzzleSlot> AllSlots = grid.Slots;

        chosenSlots = grid.GetChosenSlots();
        Slots = grid.Slots;
        checkCanGoChosenNode = grid.GetHidedSecondGrid();
        // chosenSlots = AllSlots.Where(x => x.isChosenSlot).ToList();
        // Slots = AllSlots.Where(x => !x.isChosenSlot && !x.GetComponent<GridNode>().Blocked && x.gameObject.activeSelf).ToList();
        isPuzzling = true;
        CheckAllBotPaths();
    }

    internal void StopPuzzle()
    {
        isPuzzling = false;
        // if (currentBusStop)
        // {

        //     currentBusStop.ShowPuzzle(false);
        // }
    }

    internal List<Bot> GetClickAbleBots()
    {
        List<Bot> SeatchBots = Slots.Where(x => x.GetBot() != null).Select(x => x.Bot).ToList();
        var pair = CalcColors(SeatchBots);
        return Slots.Where(x =>
        !x.isChosenSlot &&
        x.gameObject.activeSelf &&
        !x.GetComponent<GridNode>().Blocked &&
        x.GetBot() != null &&
        x.GetBot()?.GetColor() == pair.Key).Select(x => x.Bot).ToList();
    }
    // Define a class to represent nodes in the grid for the A* algorithm
}

