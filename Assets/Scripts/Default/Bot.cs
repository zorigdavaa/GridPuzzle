using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot : Character, IPuzzleObj
{
    public Transform Target;
    [SerializeField] Transform Chest;
    public PuzzleSlot currentSlot { get; set; }
    public List<Vector3> Paths { get; set; }
    public int CurrentPathIndex { get; set; } = 0;
    public Color startColor;
    public bool ChosenState { get; set; }
    QueueManager queueManager;
    private void Start()
    {
        queueManager = FindObjectOfType<QueueManager>();
    }
    IQItem PickItem;

    public event EventHandler OnClicked;

    void Update()
    {
        if (HasPath())
        {
            FollowPath();
        }
        else if (ChosenState)
        {
            print("SSSS");
            foreach (var item in queueManager.Queues)
            {
                if (item.GetFirst()?.GetColor() == GetColor())
                {
                    PickItem = item.GetFirst();
                    Paths = new List<Vector3> { item.transform.position };
                    OnPathComplete = () =>
                    {
                        Debug.Log("First Complete");
                        IQItem qItem = item.Deque();
                        qItem.transform.SetParent(transform);
                        qItem.transform.position += Vector3.up * 1.5f;
                        Paths = new List<Vector3> { transform.position + Vector3.right * 20 };
                        currentSlot.SetBot(null);
                        OnPathComplete = () =>
                        {
                            Destroy(gameObject);
                        };
                    };
                    break;
                }
            }

        }
    }

    public bool HasPath()
    {
        return Paths != null && Paths.Count > 0 && CurrentPathIndex < Paths.Count;
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
                animationController.Idle();
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
            animationController.Walk();
        }
    }

    private void MovtoTarget(Vector3 currentTarget, Action NearAction)
    {
        // Vector3 currentTarget = paths[currentPathIndex];
        float distance = Vector3.Distance(transform.position, currentTarget);
        if (distance < 0.1f)
        {
            NearAction();
        }
        else
        {
            Vector3 SameY = transform.position;
            SameY.y = currentTarget.y;
            Vector3 dir = (currentTarget - SameY).normalized;
            MoveTo(dir);
            animationController.Walk();
        }
    }
    public Action OnPathComplete { get; set; }

    public void GotoPath(List<Vector3> paths, Action action)
    {
        this.Paths = paths;
        OnPathComplete = action;
    }

    public override void Die()
    {
        base.Die();
        // rb.isKinematic = true;
    }

    public void TurnOnOutline(bool val)
    {

    }

    // internal void GotoSlot(PuzzleSlot goSlot, List<Vector3> paths)
    public void GotoSlot(GridNode node, List<Vector3> paths, Action afterAction = null)
    {
        // transform.position = goSlot.transform.position;
        // print(paths.Count);

        currentSlot = node.Slot;
        GotoPath(paths, afterAction);
        node.GetComponent<PuzzleSlot>().SetBot(this, false);
        // goSlot.SetBot(this);
    }
    public void Stop()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public void Clicked(object sender, EventArgs e)
    {
        OnClicked?.Invoke(sender, e);
    }
}
