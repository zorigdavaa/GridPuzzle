using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QItem : MonoBehaviour, IColored
{
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Color GetColor()
    {
        return rend.material.color;
    }

    public void SetColor(Color color)
    {
        rend.material.color = color;
    }
}
