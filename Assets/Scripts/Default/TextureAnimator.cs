using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimator : MonoBehaviour
{
    MeshRenderer meshRendere;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        meshRendere = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        meshRendere.material.mainTextureOffset = new Vector2(0,speed * Time.time);
    }
}
