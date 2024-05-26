using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] List<Product> Products;
    // Start is called before the first frame update
    void Start()
    {
        // foreach (var item in Products)
        // {
        //     productRecipes.Add();
        // }
    }
    public bool IsMergeAble(Ingredient a, Ingredient b)
    {
        foreach (var prod in Products)
        {
            if (prod.Ingredients.Any(x => x.Type == a.Type) && prod.Ingredients.Any(x => x.Type == b.Type))
            {
                return true;
            }
        }
        return false;
    }

    public Product Merge(Ingredient a, Ingredient b)
    {
        foreach (var recipe in Products)
        {
            if (recipe.Ingredients.Any(x => x.Type == a.Type) && recipe.Ingredients.Any(x => x.Type == b.Type))
            {
                Product InsProd = Instantiate(recipe, transform.position, Quaternion.identity);
                Ingredient First = InsProd.Ingredients.Where(x => x.Type == a.Type).First();
                Ingredient Second = InsProd.Ingredients.Where(x => x.Type == b.Type).First();
                // Product newProduct = new Product();
                // newProduct.Type = recipe.Key;
                // newProduct.Ingredients = new List<Ingredient> { a, b };
                // return newProduct;
                First.gameObject.SetActive(true);
                Second.gameObject.SetActive(true);
                PuzzleSlot slot = a.currentSlot;
                slot.SetBot(InsProd);
                Destroy(a.gameObject);
                Destroy(b.gameObject);

                return InsProd;
            }
        }
        return null;
    }
    // public void Merge(Product a, Ingredient b)
    // {

    //     if (productRecipes[a.Type].Contains(b.Type) && !a.Ingredients.Contains(b))
    //     {
    //         a.AddIngredient(b);
    //     }
    // }
}
