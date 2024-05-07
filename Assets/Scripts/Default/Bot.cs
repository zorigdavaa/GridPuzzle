using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot : Character
{
    public Transform Target;
    [SerializeField] Transform Chest;
    public PuzzleSlot currentSlot;
    public List<Vector3> paths = null;
    int currentPathIndex = 0;
    public Color startColor;
    private void Start()
    {

    }
    void Update()
    {
        if (paths != null && paths.Count > 0 && currentPathIndex < paths.Count)
        {
            Vector3 currentTarget = paths[currentPathIndex];
            float distance = Vector3.Distance(transform.position, currentTarget);
            Vector3 SameY = transform.position;
            SameY.y = currentTarget.y;
            Vector3 dir = (currentTarget - SameY).normalized;
            if (distance < 0.2f)
            {
                currentPathIndex++;
                if (currentPathIndex >= paths.Count)
                {
                    //A.BusController.GetComponent<PuzzleController>().CheckToRefill();
                    //Debug.Log("Checked");

                    Stop();
                    animationController.Idle();
                    paths = null;
                    OnPathComplete?.Invoke();
                    OnPathComplete = null;
                }
            }
            else
            {
                MoveTo(dir);
                animationController.Walk();
            }
        }
    }


    public void GotoTarget()
    {
        movement.GoToPosition(Target);
    }
    public void GotoPos(Vector3 pos)
    {
        movement.GoToPosition(pos);
    }
    public void GotoPath(List<Vector3> path)
    {
        // movement.GotoPath(path);
    }

    public override void Die()
    {
        base.Die();
        // rb.isKinematic = true;
    }
    public EventHandler OnBotClicked;
    internal void Clicked()
    {
        OnBotClicked?.Invoke(this, EventArgs.Empty);
    }
    public void TurnOnOutline(bool val)
    {

    }
    Action OnPathComplete;
    // internal void GotoSlot(PuzzleSlot goSlot, List<Vector3> paths)
    internal void GotoSlot(GridNode node, List<Vector3> paths, Action afterAction = null)
    {
        // transform.position = goSlot.transform.position;
        // print(paths.Count);
        this.paths = paths;
        currentSlot = node.Slot;
        OnPathComplete = afterAction;
        node.GetComponent<PuzzleSlot>().SetBot(this, false);
        // goSlot.SetBot(this);
    }
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

}
