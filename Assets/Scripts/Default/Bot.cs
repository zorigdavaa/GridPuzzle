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
    public bool ChosenState;
    QueueManager queueManager;
    private void Start()
    {
        queueManager = FindObjectOfType<QueueManager>();
    }
    QItem PickItem;
    void Update()
    {
        if (paths != null && paths.Count > 0 && currentPathIndex < paths.Count)
        {
            Vector3 currentTarget = paths[currentPathIndex];
            Vector3 SameY = transform.position;
            SameY.y = currentTarget.y;
            float distance = Vector3.Distance(SameY, currentTarget);
            if (distance < 0.1f)
            {
                currentPathIndex++;
                if (currentPathIndex >= paths.Count)
                {
                    //A.BusController.GetComponent<PuzzleController>().CheckToRefill();
                    //Debug.Log("Checked");

                    Stop();
                    animationController.Idle();
                    paths = null;
                    currentPathIndex = 0;
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
        else if (ChosenState)
        {
            print("SSSS");
            foreach (var item in queueManager.Queues)
            {
                if (item.GetFirst()?.GetColor() == GetColor())
                {
                    PickItem = item.GetFirst();
                    paths = new List<Vector3> { item.transform.position };
                    OnPathComplete = () =>
                    {
                        Debug.Log("First Complete");
                        QItem qItem = item.Deque();
                        qItem.transform.SetParent(transform);
                        qItem.transform.position += Vector3.up * 1.5f;
                        paths = new List<Vector3> { transform.position + Vector3.right * 20 };
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
