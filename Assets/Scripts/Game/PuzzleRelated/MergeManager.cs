using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{

    private Dictionary<ProductType, List<IngredientType>> productRecipes;
    // Start is called before the first frame update
    void Start()
    {
        productRecipes = new Dictionary<ProductType, List<IngredientType>>()
        {
             {
                ProductType.Burger, new List<IngredientType>()
                {
                    IngredientType.Bread,IngredientType.Meat
                }
            }
        };
    }

    public Product Merge(Ingredient a, Ingredient b)
    {
        foreach (var recipe in productRecipes)
        {
            if (recipe.Value.Contains(a.Type) && recipe.Value.Contains(b.Type))
            {
                Product newProduct = new Product();
                newProduct.Type = recipe.Key;
                newProduct.Ingredients = new List<Ingredient> { a, b };
                return newProduct;
            }
        }
        return null;
    }
    public void Merge(Product a, Ingredient b)
    {

        if (productRecipes[a.Type].Contains(b.Type) && !a.Ingredients.Contains(b))
        {
            a.AddIngredient(b);
        }
    }
}
