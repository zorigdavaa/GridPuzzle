using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public List<Que> Queues;
    [SerializeField] Grid Grid;
    // Start is called before the first frame update
    void Start()
    {
        List<Color> colors = Grid.botColors;
        int Count = Grid.GridCount / Queues.Count;
        
        for (int i = 0; i < Queues.Count; i++)
        {
            Queues[i].SetCount(Count);
            // Queues[i].Colors = colors.GetRange(i * Count, Count);
            Queues[i].Init();
            Colorize(Queues[i], colors.GetRange(i * Count, Count));
        }
    }

    private void Colorize(Que queue, List<Color> Colors)
    {
        for (int i = 0; i < queue.Q.Count; i++)
        {
            queue.Q[i].transform.GetChild(0).GetComponent<Renderer>().material.color = Colors[i];
        }
    }
}
