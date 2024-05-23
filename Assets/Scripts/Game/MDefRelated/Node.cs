using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, ISlotObj
{
    [SerializeField] AnimationController animationController;
    [SerializeField] List<GameObject> Models;
    public bool IsUpgradeAble
    {
        get
        {
            return ModelIndex < Models.Count - 1;
        }
    }
    public int ModelIndex { get; set; }
    public Slot Slot { get; set; }

    Vector3 targetLocalPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void GotoLocalPos(Vector3 gopos)
    {
        targetLocalPos = gopos;
        StartCoroutine(LocalCoroutine(targetLocalPos));
        IEnumerator LocalCoroutine(Vector3 toPos)
        {
            float t = 0;
            float time = 0;
            float duration = 0.3f;
            Vector3 initialPosition = transform.localPosition;
            while (time < duration)
            {
                t = time / duration;
                time += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(initialPosition, toPos, t);
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetSpeed(float v)
    {
        // animationController.SetSpeed(v);
    }

    public void Upgrade()
    {
        Models[ModelIndex].SetActive(false);
        ModelIndex++;
        Models[ModelIndex].SetActive(true);
    }

    internal void GoForwardWar()
    {
        throw new NotImplementedException();
    }
}
