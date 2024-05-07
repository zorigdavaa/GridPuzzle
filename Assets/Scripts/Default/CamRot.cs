using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class CamRot : MonoBehaviour
{
    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.rotation;
    }
}
