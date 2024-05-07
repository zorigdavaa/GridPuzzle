using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using ZPackage.Helper;

public class ChanceSpawner : ObstacleSpawner
{
    [SerializeField] List<Transform> middleObsPoints;
    List<GameObject> InstanitatedObjects = new List<GameObject>();
    float randomValue;
    // Start is called before the first frame update
    void Start()
    {
        obstaclePoint.Shuffle();
        middleObsPoints.Shuffle();
        randomValue = UnityEngine.Random.value;
        if (randomValue < 0.5f)
        {
            InstantiateTimes(1);
            // randomObstacleNumber = UnityEngine.Random.Range(0, obstacles.Count);
            // randomPointNumber = UnityEngine.Random.Range(0, obstaclePoint.Count);
            // GameObject insObject = InstantiateObstacle(randomPointNumber, randomObstacleNumber);

            // AddToInstantiatedObjectsList(insObject);
        }
        else if (randomValue < 0.8f)
        {
            InstantiateTimes(2);
        }
        else
        {
            InstantiateTimes(3);
        }
    }

    private void InstantiateTimes(float times)
    {
        for (int i = 0; i < times; i++)
        {
            randomObstacleNumber = UnityEngine.Random.Range(0, obstacles.Count);
            Vector3 postion;
            if (randomObstacleNumber==1)
            {
                postion = obstaclePoint[i].position;
            }else
            {
                postion = middleObsPoints[i].position;
            }
            GameObject insObject = InstantiateObstacle(postion, randomObstacleNumber);
            if (TooNearOtherObject(insObject,2f))
            {
                Destroy(insObject);
            }
            AddToInstantiatedObjectsList(insObject);
        }
    }

    private bool TooNearOtherObject(GameObject insObject,float maxDistance)
    {
        foreach (var item in InstanitatedObjects)
        {
            float distance = Vector3.Distance(insObject.transform.position, item.transform.position);
            if (distance < maxDistance)
            {
                return true;
            }
        }
        return false;
    }

    private void AddToInstantiatedObjectsList(GameObject insObject)
    {
        InstanitatedObjects.Add(insObject);
    }
}
