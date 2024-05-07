using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsDisplay;
    [SerializeField] float showFPsTime = 1;
    float lastShowTime = Mathf.Infinity;
    float deltaTime;
    // Update is called once per frame
    void Update()
    {
        lastShowTime += Time.deltaTime;
        if (showFPsTime < lastShowTime)
        {
            lastShowTime = 0;
            float fps = 1 / Time.unscaledDeltaTime;
            fpsDisplay.text = fps.ToString("F0");
        }
    }
}
