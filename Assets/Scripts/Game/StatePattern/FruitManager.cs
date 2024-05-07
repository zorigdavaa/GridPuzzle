using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class FruitManager : Mb
{
    public Transform grx;
    FruitBase currentState;
    public GrowingState growingState = new GrowingState();
    public WholeState wholeState = new WholeState();
    public RottenState rottenState = new RottenState();
    // Start is called before the first frame update
    void Start()
    {
        // currentState = growingState;
        // currentState.BeginState(this);
        ChangeState(growingState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
    public void ChangeState(FruitBase state)
    {
        if (currentState != null)
        {
            currentState.EndState(this);
        }
        currentState = state;
        currentState.BeginState(this);
    }
}
