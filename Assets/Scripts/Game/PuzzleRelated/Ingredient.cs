using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public IngredientType Type;
    public Sprite icon;
    public GameObject model;
}
public enum IngredientType
{
    Meat, Bread, Lettuce, Cheese
}
