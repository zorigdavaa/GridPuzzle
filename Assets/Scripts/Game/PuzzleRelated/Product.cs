using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductType Type;
    public List<Ingredient> Ingredients;
    public void ArrangeIngredients()
    {
        for (int i = 0; i < Ingredients.Count; i++)
        {
            Ingredients[i].transform.localPosition = new Vector3(0, i * 0.5f, 0);
        }
    }

    internal void AddIngredient(Ingredient b)
    {
        Ingredients.Add(b);
        ArrangeIngredients();
    }
}
public enum ProductType
{
    Burger, Sabdwich
}
