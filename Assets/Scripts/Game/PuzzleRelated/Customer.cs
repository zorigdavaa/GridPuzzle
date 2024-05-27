using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Customer : Character, IQItem, IPathFollower
{
    public PuzzleSlot currentSlot;
    public List<Vector3> Paths { get; set; }
    public int CurrentPathIndex { get; set; } = 0;
    public List<ProductImagine> Orders = new List<ProductImagine>();
    public Image Bubble;
    PuzzleController puzzleController;
    List<PuzzleSlot> ChosenSlots;
    // Start is called before the first frame update
    void Start()
    {
        puzzleController = FindObjectOfType<PuzzleController>();
        ChosenSlots = puzzleController.GetChosenSlots();
    }
    public EventHandler OnOrderComplete;
    public bool FirstInline = false;
    void Update()
    {
        if (HasPath())
        {
            FollowPath();
        }
        else if (FirstInline)
        {
            print("SSSS");
            foreach (var item in ChosenSlots)
            {
                Product prod = item.GetPuzzleObj()?.gameObject.GetComponent<Product>();
                if (prod && HasSameIngrediend(prod, Orders[0]))
                {
                    prod.currentSlot.SetBot(null);
                    List<Vector3> path = new List<Vector3> { item.transform.position };
                    Action afterAction = () =>
                    {
                        Debug.Log("First Complete");
                        prod.transform.SetParent(transform);
                        prod.transform.position += Vector3.up * 1.5f;
                        List<Vector3> paths = new List<Vector3> { transform.position + Vector3.right * 20 };
                        GoPath(paths, () =>
                        {
                            Destroy(gameObject);
                        });
                    };
                    GoPath(path, afterAction);
                    FirstInline = false;
                    OnOrderComplete?.Invoke(this, EventArgs.Empty);
                    break;
                }
            }

        }
    }

    private bool HasSameIngrediend(Product prod, ProductImagine productImagine)
    {
        if (prod.Type == productImagine.Type && prod.ActiveIngredients.Count == productImagine.Types.Count)
        {
            foreach (var item in productImagine.Types)
            {
                if (!prod.ActiveIngredients.Any(x => x.Type == item))
                {
                    Debug.Log("Not Same Ing");
                    return false;
                }
            }
            return true;
        }

        return false;

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

    internal void ShowBubble()
    {
        Bubble.transform.parent.parent.gameObject.SetActive(true);
        Bubble.sprite = Orders[0].Image;
    }
}
