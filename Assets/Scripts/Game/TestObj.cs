using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour, ISlotObj
{
    [SerializeField] List<GameObject> Models;

    public Slot Slot { get; set; }
    public bool IsUpgradeAble
    {
        get
        {
            return ModelIndex < Models.Count - 1;
        }
    }
    public int ModelIndex { get; set; }
    Vector3 targetLocalPos = Vector3.zero;

    public void Upgrade()
    {
        Models[ModelIndex].SetActive(false);
        ModelIndex++;
        Models[ModelIndex].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Upgrade();
        }
    }
}
