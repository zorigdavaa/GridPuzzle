using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    GroundSpawner GroundSpawner;
    [SerializeField] GameObject actualFloorObject;
    // Start is called before the first frame update
    void Start()
    {
        GroundSpawner = FindObjectOfType<GroundSpawner>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Jelly")
        {
            Destroy(gameObject,5);
            GroundSpawner.GeneralTileSpawner();
        }
    }
}
