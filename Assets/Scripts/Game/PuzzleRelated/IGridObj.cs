using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridObj : IColored, IPathFollower
{
    public Transform transform { get; }
    public GameObject gameObject { get; }
    public PuzzleSlot currentSlot { get; set; }
    void GotoSlot(GridNode node, List<Vector3> paths, Action afterAction = null);
    public event EventHandler OnClicked;
    public void Clicked(object sender, EventArgs e);
    public bool ChosenState { get; set; }
}
