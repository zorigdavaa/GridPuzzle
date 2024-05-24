using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFollower
{
    void FollowPath();
    void GotoPath(List<Vector3> path, Action afterAction = null);
    bool HasPath();
    public int CurrentPathIndex { get; set; }
    public List<Vector3> Paths { get; set; }
    public Action OnPathComplete { get; set; }
}