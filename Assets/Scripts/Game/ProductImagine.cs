using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Imagine", menuName = "ScriptableObjects/ProductImagine")]
public class ProductImagine : ScriptableObject
{
    public Sprite Image;
    public ProductType Type;
    public List<IngredientType> Types;
}
