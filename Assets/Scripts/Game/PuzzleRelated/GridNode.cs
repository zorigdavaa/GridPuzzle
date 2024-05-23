using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public GridNode Parent;
    public int X;
    public int Y;
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get { return GCost + HCost; } }
    public Vector3 Position { get { return new Vector3(X, 0, Y); } }
    public bool IsTraversable { get { return GetComponent<PuzzleSlot>().GetBot() == null && !Blocked; } } // You may need to implement this depending on your grid setup

    public Grid OwnGrid { get; internal set; }

    public PuzzleSlot Slot;
    public Vector3 Offset;
    public bool Blocked = false;

    internal PuzzleSlot GetLeftObj()
    {
        // if (OwnGrid == null)
        // {
        //     OwnGrid = transform.parent.parent.GetComponent<Grid>();
        // }
        return OwnGrid.GetGridObject(X - 1, Y).GetComponent<PuzzleSlot>();
    }
    // public GridNode(GridNode parent, int x, int y)
    // {
    //     Parent = parent;
    //     X = x;
    //     Y = y;
    // }
}
