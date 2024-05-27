using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Product : MonoBehaviour, IPuzzleObj, IMergeAble
{
    public ProductType Type;
    public List<Ingredient> Ingredients;
    public List<Ingredient> ActiveIngredients;
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
    public void ArrangeIngredients()
    {
        for (int i = 0; i < Ingredients.Count; i++)
        {
            Ingredients[i].transform.localPosition = new Vector3(0, i * 0.5f, 0);
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

    public void FollowPath()
    {
        throw new NotImplementedException();
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

    internal void AddIngredient(Ingredient b)
    {
        Ingredients.Add(b);
        ArrangeIngredients();
    }
    [SerializeField] List<ProductImagine> Imagines;
    internal ProductImagine GetRandomImage()
    {
        return Imagines[Random.Range(0, Imagines.Count)];
    }
}



public enum ProductType
{
    Burger, Sabdwich
}
