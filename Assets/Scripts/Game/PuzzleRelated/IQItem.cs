using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQItem : IColored
{
    public Transform transform { get; }
    public GameObject gameObject { get; }
    public void GoToQPos(Que Q);
}
