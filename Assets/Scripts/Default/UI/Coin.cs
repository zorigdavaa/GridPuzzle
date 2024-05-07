using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Coin : MonoBehaviour
{
    public int Amount;
    float rotateSpeed;
    Transform rotChild;
    private void Start()
    {
        rotChild = transform.GetChild(0);
        // rotateSpeed = Settings.Instance.CoinRotationSpeed;
        rotateSpeed = 45;
    }
    // Update is called once per frame
    void Update()
    {
        rotChild.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Coin++;
            Destroy(gameObject);
        }
    }

}
