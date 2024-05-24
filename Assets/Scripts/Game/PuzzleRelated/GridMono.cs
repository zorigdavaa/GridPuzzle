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

public class GridMono : MonoBehaviour
{
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
    [SerializeField] List<GridNode> Nodes;

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
                    GridNode node = Nodes[index];
                    node.OwnGrid = this;
                    node.X = x;
                    node.Y = y;
                    gridArray[x, y] = node;
                    node.transform.position = GetWorldPosition(x, y);
                    index++;
                }
            }
        }

    }
    [ContextMenu("Placement")]
    public void Placement()
    {
        int index = 0;
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                GridNode node = Nodes[index];
                node.X = x;
                node.Y = y;
                node.transform.SetSiblingIndex(index);
                node.transform.position = GetWorldPosition(x, y);
                index++;
            }
        }
    }

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
    }

    public int GetDistance(GridNode nodeA, GridNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);
        return dstX + dstY;
    }

    internal GridNode GetHidedSecondGrid()
    {
        return GetGridObject(3, 1);
    }
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
            List<GridNode> neighbors = GetNeighbors(currentNode);
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
                int newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);

                // If the new G cost is lower than the neighbor's current G cost or if the neighbor is not in the open list
                if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    // Update the neighbor's G cost and H cost
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, targetPos);

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
        path.Add(GetWorldPosition(currentNode.X, currentNode.Y));
        while (currentNode != startNode)
        {
            // path.Add(currentNode.Position);
            path.Add(GetWorldPosition(currentNode.X, currentNode.Y));
            // print(" Position was " + currentBusStop.Grid.GetWorldPosition(currentNode.X, currentNode.Y));
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }
}

