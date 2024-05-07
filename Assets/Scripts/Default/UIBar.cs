using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] Image BarRight;
    [SerializeField] Image BarLeft;
    [SerializeField] Image Fill;
    [SerializeField] Color fillColor;
    [SerializeField] Color backGroundColor;

    // Start is called before the first frame update
    void Start()
    {
        Initcolor();
    }

    private void Initcolor()
    {
        BarLeft.color = backGroundColor;
        BarRight.color = backGroundColor;
        Fill.color = fillColor;
    }
    public void FillHealthBar(float value)
    {
        Fill.fillAmount = value;
    }
}
