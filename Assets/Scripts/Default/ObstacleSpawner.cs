using ZPackage.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacles;
    public List<Transform> obstaclePoint;
    public bool spawnInSinglePoint = false;
    public int NumberOfObstacle;
    // Start is called before the first frame update
    public int randomObstacleNumber;
    public int randomPointNumber;
    void Start()
    {
        obstaclePoint.Shuffle();
        if (spawnInSinglePoint)
        {
            randomObstacleNumber = Random.Range(0, obstacles.Count);
            randomPointNumber = Random.Range(0, obstaclePoint.Count);
            InstantiateObstacle(randomPointNumber, randomObstacleNumber);
        }
        else
        {
            for (int i = 0; i < NumberOfObstacle; i++)
            {
                randomObstacleNumber = Random.Range(0, obstacles.Count);
                InstantiateObstacle(i, randomObstacleNumber);
            }
        }

    }

    public GameObject InstantiateObstacle(int obstaclePointIndex, int obstacleIndex)
    {
        GameObject obstacle = Instantiate(obstacles[obstacleIndex], obstaclePoint[obstaclePointIndex].position, Quaternion.identity);
        obstacle.transform.SetParent(transform);
        return obstacle;
    }
    public GameObject InstantiateObstacle(Vector3 position, int obstacleIndex)
    {
        GameObject obstacle = Instantiate(obstacles[obstacleIndex], position, Quaternion.identity);
        obstacle.transform.SetParent(transform);
        return obstacle;
    }
    public GameObject InstantiateObstacle(Vector3 position, GameObject ToInsObject)
    {
        GameObject obstacle = Instantiate(ToInsObject, position, Quaternion.identity);
        obstacle.transform.SetParent(transform);
        return obstacle;
    }
}
