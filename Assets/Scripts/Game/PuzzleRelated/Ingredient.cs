using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour, IPuzzleObj, IMergeAble
{
    public IngredientType Type;
    public Sprite icon;
    public GameObject model;
    public int CurrentPathIndex { get; set; } = 0;
    public List<Vector3> Paths { get; set; }
    public Action OnPathComplete { get; set; }
    public PuzzleSlot currentSlot { get; set; }
    public event EventHandler OnClicked;
    public float speed = 5;
    [SerializeField] Renderer rend;
    public Renderer Rend
    {
        get { return rend; }
        // set { rend = value; }
    }

    public bool ChosenState { get; set; }

    void Update()
    {
        if (HasPath())
        {
            FollowPath();
        }
    }
    public void FollowPath()
    {
        Vector3 currentTarget = Paths[CurrentPathIndex];
        Vector3 SameY = transform.position;
        SameY.y = currentTarget.y;
        float distance = Vector3.Distance(SameY, currentTarget);
        if (distance < 0.2f)
        {
            CurrentPathIndex++;
            if (CurrentPathIndex >= Paths.Count)
            {
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
        }
    }

    public void GotoPath(List<Vector3> path, Action afterAction = null)
    {
        this.Paths = path;
        OnPathComplete = afterAction;
    }

    public bool HasPath()
    {
        return Paths != null && Paths.Count > 0 && CurrentPathIndex < Paths.Count;
    }




    public void MoveTo(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            // rb.velocity = dir * speed;
            // transform.Translate(transform.position + dir * speed * Time.deltaTime);
            transform.position = transform.position + dir * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void GotoSlot(GridNode node, List<Vector3> paths, Action afterAction = null)
    {
        currentSlot = node.Slot;
        GotoPath(paths, afterAction);
        node.GetComponent<PuzzleSlot>().SetBot(this, false);
    }

    public Color GetColor()
    {
        return Rend.material.color;
    }

    public void SetColor(Color color)
    {
        Rend.material.color = color;
    }

    public void Clicked(object sender, EventArgs e)
    {
        OnClicked?.Invoke(sender, e);
    }
}
public enum IngredientType
{
    Meat, Bread, Lettuce, Cheese, Tomato
}
