using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenState : FruitBase
{
    float time = 3;
    public override void BeginState(FruitManager manager)
    {
        manager.grx.GetComponent<Renderer>().materials[0].color = Color.black;
    }

    public override void EndState(FruitManager manager)
    {
        MonoBehaviour.Destroy(manager.gameObject);
    }

    public override void UpdateState(FruitManager manager)
    {
        if (manager.grx.localScale.x > 0.3f)
        {
            manager.grx.localScale -= (Vector3.one * 0.1f) * Time.deltaTime;
        }
        else
        {
            manager.ChangeState(manager.growingState);
        }
    }
}
