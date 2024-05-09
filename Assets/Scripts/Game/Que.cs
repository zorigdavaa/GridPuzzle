using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Que : MonoBehaviour
{
    public List<GameObject> Q = new List<GameObject>();
    Dictionary<GameObject, Vector3> QPos = new Dictionary<GameObject, Vector3>();
    [SerializeField] List<GameObject> Pfs;
    [SerializeField] int Count = 10;
    // public List<Color> Colors;
    float qOffset = 1;
    // Start is called before the first frame update
    void Start()
    {
        // Init();
    }

    public void Init()
    {
        for (int i = 0; i < Count; i++)
        {
            Instantiate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(Deque());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate();
        }
    }
    public void SetCount(int count)
    {
        Count = count;
    }

    private void Instantiate()
    {
        GameObject pf = Pfs[Random.Range(0, Pfs.Count)];
        GameObject obj = Instantiate(pf, transform.position, Quaternion.identity, transform);
        Enque(obj);
    }

    public void Enque(GameObject obj)
    {
        Q.Add(obj);
        if (Q.Count == 1)
        {
            // If the queue is empty (this is the first object), set its position based on the current position and offset
            // QPos.Add(obj, transform.position + (-transform.forward * qOffset));
            QPos.Add(obj, transform.position);
        }
        else
        {
            // If the queue is not empty, set its position based on the position of the last object in the queue
            GameObject lastObject = Q[Q.Count - 2];
            QPos.Add(obj, QPos[lastObject] + (-transform.forward * qOffset));
        }
        obj.transform.position = QPos[obj];
        // insPos = QPos[obj] + (-transform.forward * qOffset);
        // insPos -= transform.forward * qOffset;
    }
    public GameObject Deque()
    {
        if (Q.Count == 0)
            return null;

        GameObject obj = Q[0];
        Q.Remove(obj);
        QPos.Remove(obj);
        // insPos += transform.forward * qOffset;
        // insPos = QPos[obj] + (transform.forward * qOffset);
        for (int i = 0; i < Q.Count; i++)
        {
            QPos[Q[i]] += transform.forward * qOffset;
        }
        RePosition();
        return obj;
    }
    public GameObject GetFirst()
    {
        if (Q.Count == 0)
            return null;

        GameObject obj = Q[0];
        return obj;
    }

    Coroutine corot;
    public void RePosition()
    {
        if (corot != null)
        {
            StopCoroutine(corot);
        }
        if (Q.Any())
        {

            corot = StartCoroutine(LocalCoroutine2());
        }
        // IEnumerator LocalCoroutine()
        // {
        //     while (Vector3.Distance(QPos[Q[0]], Q[0].transform.position) > 0.1f)
        //     {
        //         foreach (var item in Q)
        //         {
        //             item.transform.position += transform.forward * 3 * Time.deltaTime;
        //         }
        //         // insPos = Q[Q.Count - 1].transform.position - (transform.forward * qOffset);
        //         yield return null;
        //     }
        //     corot = null;
        // }
        IEnumerator LocalCoroutine2()
        {
            float t;
            float time = 0;
            float duration = 1f;
            // Vector3 initScale = transform.localScale;  // Replace with your initial scale
            // Vector3 toScale = new Vector3(2f, 2f, 2f);  // Replace with your target scale

            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                foreach (var item in Q)
                {
                    item.transform.position = Vector3.Lerp(item.transform.position, QPos[item], t);
                }
                yield return null;
            }

            // Ensure that the final scale is exactly the target scale
            // transform.localScale = toScale;
        }
    }
}
