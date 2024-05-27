using System;
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
            if (ingA.Type == IngB.Type)
            {
                return false;
            }
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

    public void Merge(IMergeAble a, IMergeAble b)
    {
        if (a is Ingredient ingA && b is Ingredient IngB)
        {
            foreach (var recipe in Products)
            {
                if (recipe.Ingredients.Any(x => x.Type == ingA.Type) && recipe.Ingredients.Any(x => x.Type == IngB.Type))
                {

                    PuzzleSlot Bslot = IngB.currentSlot;
                    Bslot.SetBot(null);
                    StartCoroutine(MergingCor(ingA.transform, IngB.transform, () =>
                    {
                        Product InsProd = Instantiate(recipe, transform.position, Quaternion.identity);
                        Ingredient First = InsProd.Ingredients.Where(x => x.Type == ingA.Type).First();
                        Ingredient Second = InsProd.Ingredients.Where(x => x.Type == IngB.Type).First();
                        InsProd.ActiveIngredients.Add(First);
                        InsProd.ActiveIngredients.Add(Second);
                        First.gameObject.SetActive(true);
                        Second.gameObject.SetActive(true);
                        PuzzleSlot Aslot = ingA.currentSlot;
                        Aslot.SetBot(InsProd);

                        Destroy(ingA.gameObject);
                        Destroy(IngB.gameObject);
                        puzzleController.Merge();

                    }));
                    // return InsProd;
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

        // return null;
    }

    private IEnumerator MergingCor(Transform A, Transform B, Action afterAction)
    {
        float time = 0;
        float dur = 0.5f;
        float t;
        Vector3 initPos = B.transform.position;
        Vector3 TargetPos = A.transform.position;
        while (time < dur)
        {
            time += Time.deltaTime;
            t = time / dur;
            initPos.y += 4 * Time.deltaTime;
            B.transform.position = Vector3.Lerp(initPos, TargetPos, t);
            yield return null;
        }
        afterAction();

    }

    public void Merge(Product a, Ingredient b)
    {
        Ingredient sameIng = a.Ingredients.Where(x => x.Type == b.Type).FirstOrDefault();
        // if (!sameIng.gameObject.activeSelf)
        if (!a.ActiveIngredients.Contains(sameIng))
        {
            StartCoroutine(MergingCor(a.transform, b.transform, () =>
            {
                sameIng.gameObject.SetActive(true);
                a.ActiveIngredients.Add(sameIng);
                PuzzleSlot slot = b.currentSlot;
                slot.SetBot(null);
                Destroy(b.gameObject);
                puzzleController.Merge();

            }));
        }
    }
}
