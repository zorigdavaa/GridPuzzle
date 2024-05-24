using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Character, IQItem, IPathFollower
{
    public PuzzleSlot currentSlot;
    public List<Vector3> Paths { get; set; }
    public int CurrentPathIndex { get; set; } = 0;
    public List<Product> Orders = new List<Product>();
    // Start is called before the first frame update
    void Start()
    {

    }

    IQItem PickItem;
    void Update()
    {
        if (HasPath())
        {
            FollowPath();
        }
    }
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }
    public Action OnPathComplete { get; set; }
    internal void GoPath(List<Vector3> paths, Action afterAction = null)
    {
        this.Paths = paths;
        OnPathComplete = afterAction;
    }

    public void GoToQPos(Que Q)
    {
        List<Vector3> path = new List<Vector3>()
        {
            transform.position, Q.GetPos(this)
        };
        GoPath(path);
    }

    public void FollowPath()
    {
        Vector3 currentTarget = Paths[CurrentPathIndex];
        Vector3 SameY = transform.position;
        SameY.y = currentTarget.y;
        float distance = Vector3.Distance(SameY, currentTarget);
        if (distance < 0.1f)
        {
            CurrentPathIndex++;
            if (CurrentPathIndex >= Paths.Count)
            {
                //A.BusController.GetComponent<PuzzleController>().CheckToRefill();
                //Debug.Log("Checked");

                Stop();
                // animationController.Idle();
                Paths = null;
                CurrentPathIndex = 0;
                OnPathComplete?.Invoke();
                // OnPathComplete = null;
                print("Path End");
            }
        }
        else
        {

            Vector3 dir = (currentTarget - SameY).normalized;
            MoveTo(dir);
            // animationController.Walk();
        }
    }

    public void GotoPath(List<Vector3> path, Action afterAction = null)
    {
        Paths = path;
        OnPathComplete = afterAction;
    }

    public bool HasPath()
    {
        return Paths != null && Paths.Count > 0 && CurrentPathIndex < Paths.Count;
    }
}
