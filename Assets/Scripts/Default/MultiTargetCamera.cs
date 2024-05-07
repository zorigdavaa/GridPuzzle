using UnityEngine;
using ZPackage;
using System.Collections.Generic;
using System;
using System.Collections;

public class MultiTargetCamera : Mb
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    Vector3 velocity = Vector3.zero;
    [SerializeField] float maxSpeed = 30f;
    [SerializeField]
    [Range(0.01f, 1f)]
    float smoothTime = 0.3f;
    public List<Transform> targets;

    void OnGameStart()
    {
        transform.position = new Vector3(7, 15, -22);
    }
    Vector3 centerPoint;
    private void FixedUpdate()
    {
        if (targets.Count == 0)
            return;
        
        centerPoint = GetCenterPoint();
        Vector3 targetPos = centerPoint + offset;
        targetPos.x = transform.position.x;
        if (IsPlaying)
        {
            targetPos.y = transform.position.y;
            //transform.position = targetPos;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime,maxSpeed);
        }
    }
    public void addToList(Transform target)
    {
        targets.Add(target);
    }
    public void removeFromList(Transform target)
    {
        targets.Remove(target);
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position,Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    public void SetOffset(Vector3 target, float duration = 2.0f)
    {
        StartCoroutine(SetOffsetCoroutine(target, duration));
        //offset = target;
        //while (offset != target)
        //{
        //offset = Vector3.SmoothDamp(offset, target, ref velocity, smoothTime);
        //}

    }
    IEnumerator SetOffsetCoroutine(Vector3 target, float duration = 2.0f)
    {
        float time = 0;
        Vector3 startingOffset = offset;
        while (time < duration)
        {
            float t = time / duration;
            Vector3 offsets = Vector3.Lerp(startingOffset, target, t);
            offset = offsets;
            time += Time.deltaTime;
            yield return null;
        }
    }
    public void AddValueToOffset(Vector3 addingV3)
    {
        var taret = offset += addingV3;
        SetOffset(taret);
    }
    public void MinusValueFromOffset(Vector3 minusV3)
    {
        var taret = offset -= minusV3;
        SetOffset(taret);
    }
}