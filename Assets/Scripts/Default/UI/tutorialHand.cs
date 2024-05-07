using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialHand : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField] float speed = 1.5f;
    // Update is called once per frame
    void Update()
    {
        var t = Time.time * speed;
        rectTransform.localPosition = Vector3.Lerp(pos1, pos2, Mathf.PingPong(t, 1.0f));

    }
}
