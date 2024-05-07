using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] List<GameObject> Models;
    [SerializeField] GameObject sum;
    Slot mySlot;
    int curModelsIndex = 0;
    public int HighestModel => Models.Count - 1;
    LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }
    public void UpGrade()
    {
        Models[curModelsIndex].SetActive(false);
        curModelsIndex++;
        Models[curModelsIndex].SetActive(true);
    }
    public void SetSlot(Slot slot)
    {
        mySlot = slot;
    }
    public Slot GetSlot()
    {
        return mySlot;
    }
    public int GetModelIndex()
    {
        return curModelsIndex;
    }
    public bool UpgradeAble()
    {
        return curModelsIndex < HighestModel;
    }
    public float shotTime = 3;
    public float shotPower = 200;
    float shootTImer = 3;
    private void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5, enemyLayer);
        shootTImer -= Time.deltaTime;
        if (enemies.Length > 0 && shootTImer < 0)
        {
            shootTImer = shotTime;
            Shoot(enemies[0]);
        }
    }

    private void Shoot(Collider collider)
    {
        GameObject insSum = Instantiate(sum, transform.position + Vector3.up * 2, Quaternion.identity);
        Vector3 dir = collider.transform.position - transform.position;
        insSum.GetComponent<Rigidbody>().AddForce(dir.normalized * shotPower);
        Destroy(insSum, 2);
    }
}
