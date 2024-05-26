using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public List<Que> Queues;
    [SerializeField] GridMono Grid;

    // Start is called before the first frame update
    void Start()
    {
        MergeManager mergeManager = FindObjectOfType<MergeManager>();
        for (int i = 0; i < Queues.Count; i++)
        {
            Queues[i].SetCount(10);
            Queues[i].Init();
            Queues[i].SetOrder(mergeManager.Products);
        }
    }
}
