using ZPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZPackage
{

    public class CameraController : GenericSingleton<CameraController>
    {
        [SerializeField] Transform followTf;
        [SerializeField] CamerShaker shaker;
        Vector3 offset;
        Vector3 velocity = Vector3.zero;
        [SerializeField][Range(0.01f, 1f)] float smoothTime = 0.125f;
        Camera cam;

        [Header("Zoom")]
        public float zoomDuration = 1;
        public float zoomMagnitude = 10;
        public static bool IsZoom = false;

        private void Awake()
        {
            // player = FindObjectOfType<PlayerMovement>().transform;
            //offset = new Vector3(6, 14, -19);
            offset = transform.position - followTf.position;
            cam = transform.GetChild(0).GetComponent<Camera>();
        }
        void OnGameStart()
        {
            transform.position = new Vector3(7, 15, -22);
        }
        private void LateUpdate()
        {
            if (followTf)
            {
                Vector3 targetPos = followTf.position + offset;
                // transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
                transform.position = targetPos;
            }
        }
        // private void FixedUpdate()
        // {
        //     if (followTf)
        //     {
        //         Vector3 targetPos = followTf.position + offset;
        //         // transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //         transform.position = targetPos;
        //     }

        //     //else if(Flying)
        //     //{
        //     //    transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //     //}
        // }
        public void Follow(Transform follow)
        {
            followTf = follow;
        }
        Coroutine offsetCoroutine;
        public void SetOffset(Vector3 offset, Vector3 rotation, float time)
        {
            if (offsetCoroutine != null)
            {
                StopCoroutine(offsetCoroutine);
            }
            offsetCoroutine = StartCoroutine(SetOffsetCor(offset, rotation, time));
            IEnumerator SetOffsetCor(Vector3 IncomingOffset, Vector3 IncomingRotation, float duration)
            {
                float time = 0;
                Vector3 startingOffset = offset;
                Vector3 startingRotation = transform.eulerAngles;
                Quaternion startQuaternin = transform.rotation;
                Quaternion toRoataion = Quaternion.Euler(IncomingRotation);
                while (time < duration)
                {
                    time += Time.deltaTime;
                    offset = Vector3.Lerp(startingOffset, IncomingOffset, time / duration);
                    transform.rotation = Quaternion.Lerp(startQuaternin, toRoataion, time / duration);
                    //  transform.eulerAngles = Vector3.Lerp(startingRotation,IncomingRotation,time/duration);
                    yield return null;
                }
                offset = IncomingOffset;
                // transform.eulerAngles = IncomingRotation;
            }
        }

        public void Shake(float lenght = 0.5f, float power = 0.3f)
        {
            // shaker.ShakeCor();
            shaker.ShakeLateUpdate(lenght, power);
        }

        public void SetOffset(Vector3 target, float duration = 2.0f)
        {
            StartCoroutine(SetOffsetCoroutine(target, duration));
            //offset = target;
            //while (offset != target)
            //{
            //offset = Vector3.SmoothDamp(offset, target, ref velocity, smoothTime);
            //}
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
        }
        Coroutine zoomCoroutine;
        public void Zoom(float duration = 1, float to = 20, Action afterAction = null)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(LocalFunction());
            IEnumerator LocalFunction()
            {
                if (cam.fieldOfView == to)
                {
                    if (afterAction != null)
                    {
                        afterAction();
                    }
                    yield break;
                }
                float time = 0;
                float initialvalue = cam.fieldOfView;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    cam.fieldOfView = Mathf.Lerp(initialvalue, to, time / duration);
                    yield return null;
                }
                if (afterAction != null)
                {
                    afterAction();
                }
            }
        }
        public void ZoomToWard(float value, float speed = 0.3f)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, value, speed);
        }
        public void ZoomInstant(float value = 60)
        {
            cam.fieldOfView = value;
        }
        // public void Zoom(Vector3 pos)
        // {
        //     StartCoroutine(Zoom(pos, Vector3.Distance(transform.position, pos) / zoomMagnitude * zoomDuration));
        // }
        // IEnumerator Zoom(Vector3 pos, float duration)
        // {
        //     if (!IsZoom)
        //     {
        //         IsZoom = true;
        //         Vector3 startPos = transform.position;
        //         for (float t = 0; t < duration; t += DT)
        //         {
        //             transform.position = Vector3.Lerp(startPos, pos, t / duration);
        //             yield return null;
        //         }
        //         transform.position = pos;
        //         IsZoom = false;
        //     }
        // }


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
}
