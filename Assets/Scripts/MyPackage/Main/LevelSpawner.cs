using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ZPackage.Utility;

namespace ZPackage
{
    public class LevelSpawner : GenericSingleton<LevelSpawner>
    {
        [SerializeField] List<GameObject> Levels;
        [SerializeField] GameObject Man;
        [SerializeField] GameObject Scores;
        List<Vector3> points;

        float lastMultPos = 35;
        private void Start()
        {
            // InstantiateBot(10);
        }
        private void InstantiateBot(int v)
        {
            for (int i = 0; i < v; i++)
            {
                Instantiate(Man, new Vector3(0, 2, lastMultPos), Quaternion.identity, transform);
                lastMultPos += 60;
            }
        }
    }
}

