using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Random = UnityEngine.Random;
using System.Linq;
using UnityUtilities;
using Unity.VisualScripting;

public class Grid : MonoBehaviour
{
    public List<Color> slotBotColors;
    //public List<Color> testBotColors;
    [SerializeField] GameObject botPf;
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    public Transform origin;
    [SerializeField] int width;
    [SerializeField] int height;
    float cellSize = 1f;
    private GridNode[,] gridArray;
    public List<PuzzleSlot> Slots;
    public Transform OffPos;
    public int GridCount;
    public int version;



    // public Grid(int width, int height, float cellSize, Transform parent, List<PuzzleSlot> ordered)
    // {
    //     this.width = width;
    //     this.height = height;
    //     this.cellSize = cellSize;
    //     this.originPosition = parent.transform.position;
    //     this.parent = parent;

    //     gridArray = new GridNode[width, height];
    //     int index = 0;
    //     for (int y = 0; y < gridArray.GetLength(1); y++)
    //     {
    //         for (int x = 0; x < gridArray.GetLength(0); x++)
    //         {
    //             // object created = createGridObject(this, x, y);
    //             // bool isChosen = GameController.SidePuzzle ? x == 0 : y == 5;
    //             bool isChosen = x == 0;
    //             PuzzleSlot slot = ordered[index];
    //             GridNode node = slot.gameObject.AddComponent<GridNode>();
    //             slot.isChosenSlot = isChosen;
    //             if (isChosen)
    //             {
    //                 Color color = isChosen ? Color.white : new Color32(190, 201, 208, 255);
    //                 slot.SetColor(color);
    //                 // node.Offset = Vector3.zero;
    //             }
    //             else
    //             {
    //                 // node.Offset = node.transform.forward * cellSize;
    //             }


    //             //slot.GetBot().TurnOnOutline(true);

    //             // GridNode casted = (GridNode)created;
    //             // gridArray[x, y] = createGridObject(this, x, y);
    //             node.X = x;
    //             node.Y = y;
    //             gridArray[x, y] = node;
    //             ordered[index].GridNode = node;
    //             node.Slot = ordered[index];
    //             ordered[index].transform.position = GetWorldPosition(x, y);
    //             index++;
    //         }
    //     }


    //     // bool showDebug = true;
    //     // if (showDebug)
    //     // {
    //     //     TextMeshPro[,] debugTextArray = new TextMeshPro[width, height];

    //     //     for (int x = 0; x < gridArray.GetLength(0); x++)
    //     //     {
    //     //         for (int y = 0; y < gridArray.GetLength(1); y++)
    //     //         {
    //     //             // debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetLocalPLacement(x, y), 4, Color.white);
    //     //             Debug.DrawLine(GetLocalPLacement(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
    //     //             Debug.DrawLine(GetLocalPLacement(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
    //     //         }
    //     //     }
    //     //     Debug.DrawLine(GetLocalPLacement(0, height), GetLocalPLacement(width, height), Color.white, 100f);
    //     //     Debug.DrawLine(GetLocalPLacement(width, 0), GetLocalPLacement(width, height), Color.white, 100f);

    //     //     OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
    //     //     {
    //     //         debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
    //     //     };
    //     // }
    // }131 233 52
    // public void Initialize(List<PuzzleSlot> ordered)

    private void Awake()
    {

    }
    void Start()
    {
        Initialize();
    }

    bool initialized = false;
    public void Initialize()
    {
        if (!initialized)
        {
            initialized = true;
            gridArray = new GridNode[width, height];
            int index = 0;
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    // object created = createGridObject(this, x, y);
                    // bool isChosen = GameController.SidePuzzle ? x == 0 : y == 5;
                    bool isChosen = y == gridArray.GetLength(1) - 1;
                    bool isHideSlot = y == gridArray.GetLength(1) - 2;
                    // if (Slots.Count - 1 < index)
                    // {
                    //     Debug.Break();
                    //     print("Index was " + index);
                    //     print("Count was " + Slots.Count);
                    //     print("name was " + gameObject.name);
                    // }
                    PuzzleSlot slot = Slots[index];

                    GridNode node = slot.gameObject.GetComponent<GridNode>();
                    node.OwnGrid = this;
                    slot.isChosenSlot = isChosen;
                    Color color = isChosen ? Color.white : new Color32(202, 202, 202, 255);
                    slot.SetColor(color);
                    // if (isChosen)
                    // {
                    //     // node.Offset = Vector3.zero;
                    // }
                    // else
                    // {
                    //     slot.SetColor(color);
                    //     // node.Offset = node.transform.forward * cellSize;
                    // }
                    if (isHideSlot)
                    {
                        node.gameObject.SetActive(false);
                        node.Blocked = false;
                    }
                    node.X = x;
                    node.Y = y;
                    gridArray[x, y] = node;
                    Slots[index].GridNode = node;
                    node.Slot = Slots[index];
                    Slots[index].transform.position = GetWorldPosition(x, y);
                    slot.Init();
                    index++;
                }
            }
            RefillPuzzle();
        }

    }
    [ContextMenu("Placement")]
    public void Placement()
    {
        gridArray = new GridNode[width, height];
        int index = 0;
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                // object created = createGridObject(this, x, y);
                // bool isChosen = GameController.SidePuzzle ? x == 0 : y == 5;
                bool isChosen = y == gridArray.GetLength(0) - 1;
                bool isHideSlot = y == gridArray.GetLength(1) - 2;
                PuzzleSlot slot = Slots[index];
                GridNode node = slot.gameObject.GetComponent<GridNode>();
                slot.isChosenSlot = isChosen;
                if (isChosen)
                {
                    // Color color = isChosen ? Color.white : new Color32(190, 201, 208, 255);
                    // slot.SetColor(color);
                    // node.Offset = Vector3.zero;
                }
                else if (isHideSlot)
                {
                    node.gameObject.SetActive(false);
                    node.Blocked = false;
                }
                node.X = x;
                node.Y = y;
                slot.transform.SetSiblingIndex(index);
                gridArray[x, y] = node;
                Slots[index].transform.position = GetWorldPosition(x, y);
                index++;
            }
        }
    }

    // public Grid(int width, int height, float cellSize, Vector3 originPosition, List<TGridObject> orderedList, Transform parent)
    // {
    //     this.width = width;
    //     this.height = height;
    //     this.cellSize = cellSize;
    //     this.originPosition = originPosition;
    //     this.parent = parent;

    //     gridArray = new TGridObject[width, height];
    //     int index = 0;
    //     for (int y = 0; y < gridArray.GetLength(1); y++)
    //     {
    //         for (int x = 0; x < gridArray.GetLength(0); x++)
    //         {

    //             gridArray[x, y] = orderedList[index];
    //             orderedList[index].transform.position = GetWorldPosition(x, y);
    //             index++;
    //         }
    //     }

    //     // for (int x = 0; x < gridArray.GetLength(0); x++)
    //     // {
    //     //     for (int y = 0; y < gridArray.GetLength(1); y++)
    //     //     {

    //     //         gridArray[x, y] = orderedList[index];
    //     //         orderedList[index].transform.position = GetWorldPosition(x, y);
    //     //         index++;
    //     //     }
    //     // }

    //     // bool showDebug = true;
    //     // if (showDebug)
    //     // {
    //     //     TextMeshPro[,] debugTextArray = new TextMeshPro[width, height];

    //     //     for (int x = 0; x < gridArray.GetLength(0); x++)
    //     //     {
    //     //         for (int y = 0; y < gridArray.GetLength(1); y++)
    //     //         {
    //     //             // debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetLocalPLacement(x, y), 4, Color.white);
    //     //             Debug.DrawLine(GetLocalPLacement(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
    //     //             Debug.DrawLine(GetLocalPLacement(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
    //     //         }
    //     //     }
    //     //     Debug.DrawLine(GetLocalPLacement(0, height), GetLocalPLacement(width, height), Color.white, 100f);
    //     //     Debug.DrawLine(GetLocalPLacement(width, 0), GetLocalPLacement(width, height), Color.white, 100f);

    //     //     OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
    //     //     {
    //     //         debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
    //     //     };
    //     // }
    // }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        // // return new Vector3(x, 0, y) * cellSize + originPosition;
        // Vector3 offSet = gridArray[x, y].Offset;
        // return (new Vector3(x, 0, y) * cellSize + (origin.rotation * origin.position) + offSet);
        Vector3 offset = gridArray[x, y].Offset;
        Quaternion rotation = origin.rotation;
        Vector3 rotatedOffset = rotation * offset;
        Vector3 cellPosition = new Vector3(x, 0, y) * cellSize;
        Vector3 rotatedCellPosition = rotation * cellPosition;
        return origin.position + rotatedCellPosition + rotatedOffset;
    }
    public Vector3 GetWorldPositionNoOFF(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + origin.position;
    }
    // public Vector3 GetWorldPlacement(int x, int y)
    // {
    //     return GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
    // }
    public Vector3 GetWorldPLacement(int x, int y)
    {
        // return parent.TransformPoint(new Vector3(x, y) * cellSize + new Vector3(cellSize, cellSize) * .5f);
        return origin.TransformPoint(GetLocalPLacement(x, y));
    }
    public Vector3 GetLocalPLacement(int x, int y)
    {
        return (new Vector3(x, y) * cellSize + new Vector3(cellSize, cellSize) * .5f);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        // Vector3 offSet = gridArray[x, y].Offset;
        // x = Mathf.FloorToInt((worldPosition - origin.position).x / cellSize);
        // y = Mathf.FloorToInt((worldPosition - origin.position).y / cellSize);

        Quaternion inverseRotation = Quaternion.Inverse(origin.rotation);
        Vector3 localPosition = inverseRotation * (worldPosition - origin.position);
        x = Mathf.FloorToInt(localPosition.x / cellSize);
        y = Mathf.FloorToInt(localPosition.z / cellSize); // Assuming y is the height axis
    }

    public void SetGridObject(int x, int y, GridNode value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(object sender, int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(sender, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, GridNode value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    // public TGridObject GetGridObject(int x, int y)
    // {
    //     if (x >= 0 && y >= 0 && x < width && y < height)
    //     {
    //         return gridArray[x, y];
    //     }
    //     else
    //     {
    //         return default(TGridObject);
    //     }
    // }

    public GridNode GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
    //new Codes




    public void GetXY(GridNode gridObject, out int x, out int y)
    {
        x = Mathf.FloorToInt(gridObject.Position.x);
        y = Mathf.FloorToInt(gridObject.Position.z);
    }

    public GridNode GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(GridNode);
        }
    }

    public List<GridNode> GetNeighbors(GridNode currentNode)
    {
        List<GridNode> neighbors = new List<GridNode>();

        // Define the offsets for left, right, up, and down
        int[] xOffset = { -1, 1, 0, 0 };
        int[] yOffset = { 0, 0, 1, -1 };

        for (int i = 0; i < xOffset.Length; i++)
        {
            int checkX = currentNode.X + xOffset[i];
            int checkY = currentNode.Y + yOffset[i];

            // Check if the neighbor is within the grid bounds
            if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
            {
                GridNode neighbor = GetGridObject(checkX, checkY);

                // Check if the neighbor is traversable
                // if (neighbor != null)
                if (neighbor != null && neighbor.IsTraversable)
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;

        // for (int x = -1; x <= 1; x++)
        // {
        //     for (int y = -1; y <= 1; y++)
        //     {
        //         if (x == 0 && y == 0)
        //             continue;

        //         int checkX = currentNode.X + x;
        //         int checkY = currentNode.Y + y;

        //         if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
        //         {
        //             neighbors.Add(gridArray[checkX, checkY]);
        //         }
        //     }
        // }
    }

    public int GetDistance(GridNode nodeA, GridNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);
        return dstX + dstY;
    }
    List<PuzzleSlot> ShuffledSlots;
    internal void RefillPuzzle()
    {
        // print("Refershed  ");
        // bool isSidePuzzle = GameController.SidePuzzle;
        bool UseRandomColor = Slots[0].GetColor() == Color.black;
        // List<PuzzleSlot> ShuffledSlots = A.Shuffle(Slots);
        if (ShuffledSlots == null)
        {
            // ShuffledSlots = A.Shuffle(Slots);
            ShuffledSlots = Slots;
            ShuffledSlots.Shuffle();
            // ShuffledSlots = ShuffledSlots.Where(x => !x.GetComponent<GridNode>().Blocked).ToList();
            // ShuffledSlots = ShuffledSlots.Where(x => !x.GetComponent<GridNode>().Blocked || x is PuzzleSlotDoor).ToList();
            // puzzleSlot iis busad 
            // ShuffledSlots = ShuffledSlots.Where(x => x.gameObject.activeSelf || !x.GetComponent<GridNode>().Blocked).ToList();
            ShuffledSlots = ShuffledSlots.Where(x => x.gameObject.activeSelf).ToList();
        }

        List<Color> BotColors = new List<Color>();
        if (UseRandomColor)
        {
            BotColors = GetColorsWithMatchedCount();
            //testBotColors = GetColorsWithMatchedCount();
        }

        // print(BotColors.Count + " Count Color");
        // print(ShuffledSlots.Count + " Slots Color");
        int index = 0;
        // print(ShuffledSlots.Count + " Count is");
        foreach (var item in ShuffledSlots)
        {
            // if (!item.isChosenSlot && item.GetBot())
            if (item.GetBot() != null)
            {
                Destroy(item.GetBot().gameObject);
            }
            // Clear the bot associated with the slot
            item.SetBot(null, true);
            //
            // Determine the condition based on GameController.SidePuzzle
            // bool isHideSlot = isSidePuzzle ? (item.GetComponent<GridNode>().X == 1) : (item.GetComponent<GridNode>().Y == 4);
            GridNode node = item.GetComponent<GridNode>();
            // bool isHideSlot = node.X == 1;


            // if (isHideSlot)
            // {
            //     // Disable the slot gameObject if the condition is met
            //     item.gameObject.SetActive(false);
            // }
            if (item.GetBot() == null && !item.isChosenSlot && !node.Blocked && item is not PuzzleSlotDoor)
            {
                Color botColor;
                if (!UseRandomColor)
                {
                    botColor = item.GetColor();
                }
                else
                {
                    // botColor = slotBotColors[Random.Range(0, slotBotColors.Count)];
                    botColor = BotColors[index % BotColors.Count];
                    // botColor = BotColors[index];
                }
                InstantiateBot(botColor, item, item.isColorHidden);
                index++;
            }
            item.Refresh();
        }

        //
        //A.BusController.gameObject.GetComponent<PuzzleController>().CheckAllBotPaths();
    }

    public IGridObj InstantiateBot(Color color, PuzzleSlot item, bool isColorHidden = false)
    {
        // Quaternion rot = Quaternion.LookRotation(-transform.right);
        Quaternion rot = Quaternion.identity;
        IGridObj obj = Instantiate(botPf, item.transform.position, rot, transform).GetComponent<IGridObj>();
        item.SetBot(obj);
        // bot.SetModelIndex(-1);
        // obj.startColor = color;

        if (isColorHidden)
        {
            //Debug.Log("turned black");
            // bot.ShowIsClickable(false);
        }
        else
        {
            obj.SetColor(color);
        }
        obj.transform.localScale = Vector3.one * 0.9f;
        // bot.animationController.StopAllAnimation();

        return obj;
        //A.BusController.gameObject.GetComponent<PuzzleController>().CheckAllBotPaths();
    }
    public List<Color> botColors;

    private List<Color> GetColorsWithMatchedCount()
    {
        if (botColors.Count != GridCount)
        {
            botColors = new List<Color>();

            // for (int i = 0; i < slotBotColors.Count; i++)
            int iteratonCount = GridCount / 3;

            for (int i = 0; i < iteratonCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // botColors.Add(slotBotColors[i]);
                    botColors.Add(slotBotColors[i % slotBotColors.Count]);
                }
            }
            // botColors = A.Shuffle(botColors);
            botColors.Shuffle();
        }
        return botColors;
    }

    internal List<PuzzleSlot> GetChosenSlots()
    {
        return Slots.Where(x => x.isChosenSlot && x.gameObject.activeSelf).OrderBy(x => x.transform.position.x).ToList();
    }

    internal Vector3 GetLandintPos()
    {
        return OffPos.position + OffPos.forward * Random.Range(0, 1) + OffPos.right * Random.Range(0, 1);
    }

    internal int GetCount()
    {
        return GridCount;
    }


    [ContextMenu("SetRandomColor")]
    public void SetRandomColor()
    {
        List<PuzzleSlot> coloringSlots = Slots.Where(x => !x.isChosenSlot && x.gameObject.activeSelf && !x.GetComponent<GridNode>().Blocked).ToList();
        List<Color> toBeColors = GetColorsWithMatchedCount();
        print(coloringSlots.Count);
        print(toBeColors.Count);
        int colorIndex = 0;
        for (int i = 0; i < coloringSlots.Count; i++)
        {
            if (coloringSlots[i] is PuzzleSlotDoor door)
            {
                for (int j = 0; j < door.DefaultInsCount; j++)
                {
                    door.SetColors(toBeColors[colorIndex], j);
                    colorIndex++;
                }
            }
            else
            {
                coloringSlots[i].Color = toBeColors[colorIndex];
                colorIndex++;
            }
        }
    }

    internal GridNode GetHidedSecondGrid()
    {
        return GetGridObject(3, 1);
    }
}

