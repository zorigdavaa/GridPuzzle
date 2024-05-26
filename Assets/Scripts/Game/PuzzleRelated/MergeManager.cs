using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public List<Product> Products;
    PuzzleController puzzleController;
    // Start is called before the first frame update
    void Start()
    {
        puzzleController = FindObjectOfType<PuzzleController>();
        // foreach (var item in Products)
        // {
        //     productRecipes.Add();
        // }
    }
    public bool IsMergeAble(IMergeAble a, IMergeAble b)
    {
        if (a is Ingredient ingA && b is Ingredient IngB)
        {
            foreach (var prod in Products)
            {
                if (prod.Ingredients.Any(x => x.Type == ingA.Type) && prod.Ingredients.Any(x => x.Type == IngB.Type))
                {
                    return true;
                }
            }
        }
        else if (a is Product ProdA && b is Ingredient IngBB)
        {
            Ingredient sameIng = ProdA.Ingredients.Where(x => x.Type == IngBB.Type).FirstOrDefault();
            if (!sameIng.gameObject.activeSelf)
            {
                return true;
            }
        }
        else if (a is Ingredient IngAA && b is Product ProdB)
        {
            Ingredient sameIng = ProdB.Ingredients.Where(x => x.Type == IngAA.Type).FirstOrDefault();
            if (!sameIng.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public Product Merge(IMergeAble a, IMergeAble b)
    {
        if (a is Ingredient ingA && b is Ingredient IngB)
        {
            foreach (var recipe in Products)
            {
                if (recipe.Ingredients.Any(x => x.Type == ingA.Type) && recipe.Ingredients.Any(x => x.Type == IngB.Type))
                {
                    Product InsProd = Instantiate(recipe, transform.position, Quaternion.identity);
                    Ingredient First = InsProd.Ingredients.Where(x => x.Type == ingA.Type).First();
                    Ingredient Second = InsProd.Ingredients.Where(x => x.Type == IngB.Type).First();
                    // Product newProduct = new Product();
                    // newProduct.Type = recipe.Key;
                    // newProduct.Ingredients = new List<Ingredient> { a, b };
                    // return newProduct;
                    First.gameObject.SetActive(true);
                    Second.gameObject.SetActive(true);
                    PuzzleSlot Aslot = ingA.currentSlot;
                    PuzzleSlot Bslot = IngB.currentSlot;
                    Aslot.SetBot(InsProd);
                    Bslot.SetBot(null);
                    Destroy(ingA.gameObject);
                    Destroy(IngB.gameObject);
                    puzzleController.Merge();
                    return InsProd;
                }
            }
        }
        else if (a is Product ProdA && b is Ingredient IngBB)
        {
            Merge(ProdA, IngBB);
        }
        else if (a is Ingredient ingAA && b is Product ProdB)
        {
            Merge(ProdB, ingAA);
        }

        return null;
    }
    public void Merge(Product a, Ingredient b)
    {
        Ingredient sameIng = a.Ingredients.Where(x => x.Type == b.Type).FirstOrDefault();
        if (!sameIng.gameObject.activeSelf)
        {
            sameIng.gameObject.SetActive(true);
            PuzzleSlot slot = b.currentSlot;
            slot.SetBot(null);
            Destroy(b.gameObject);
            puzzleController.Merge();
        }
    }
}
