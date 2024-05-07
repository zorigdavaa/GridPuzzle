using UnityEngine;

public abstract class FruitBase
{
    public abstract void BeginState(FruitManager manager);
    public abstract void UpdateState(FruitManager manager);
    public abstract void EndState(FruitManager manager);
}
