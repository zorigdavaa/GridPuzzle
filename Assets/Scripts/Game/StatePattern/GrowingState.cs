using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingState : FruitBase
{
    public override void BeginState(FruitManager manager)
    {
        manager.grx.localScale = Vector3.one * 0.05f;
    }

    public override void EndState(FruitManager manager)
    {
        
    }

    public override void UpdateState(FruitManager manager)
    {
        if (manager.grx.localScale.x < 1)
        {
            manager.grx.localScale += (Vector3.one * 0.1f) * Time.deltaTime;
        }
        else
        {
            manager.ChangeState(manager.wholeState);
        }
    }
}
