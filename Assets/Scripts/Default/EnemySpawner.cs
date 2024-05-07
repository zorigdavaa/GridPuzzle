using System.Collections;
using System.Collections.Generic;
using ZPackage;
using UnityEngine;
using Unity;
using ZPackage.Helper;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] List<Transform> slots;
    public Player CorrespondingPlayer;
    [SerializeField] Vector2 minMaxEnemy;
    int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        slots.Shuffle();
        enemyCount = Random.Range((int)minMaxEnemy.x, (int)minMaxEnemy.y + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject insEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], slots[i].position, Quaternion.identity);
            // for (int j = 0; j < insEnemy.transform.childCount; j++)
            // {
            //     Enemy enemy = insEnemy.transform.GetChild(j).GetComponent<Enemy>();
            //     enemy.correspondingPlayer = CorrespondingPlayer;
            //     if (CorrespondingPlayer)
            //     {
            //         CorrespondingPlayer.correspondingEnemies.Add(enemy);
            //     }
            // }
        }
    }
}
