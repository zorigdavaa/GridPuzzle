using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeState : FruitBase
{
    float time = 3;
    public override void BeginState(FruitManager manager)
    {
        manager.rb.useGravity = true;
    }

    public override void EndState(FruitManager manager)
    {
    }

    public override void UpdateState(FruitManager manager)
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            manager.ChangeState(manager.rottenState);
        }
    }
}
