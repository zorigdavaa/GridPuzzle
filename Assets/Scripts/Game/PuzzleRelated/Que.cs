using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Que : MonoBehaviour
{
    public List<IQItem> Q = new List<IQItem>();
    Dictionary<IQItem, Vector3> QPos = new Dictionary<IQItem, Vector3>();
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
            Destroy(Deque().gameObject);
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
        IQItem obj = Instantiate(pf, transform.position, transform.rotation, transform).GetComponent<IQItem>();
        Enque(obj);
    }

    public void Enque(IQItem obj)
    {
        Q.Add(obj);
        if (Q.Count == 1)
        {
            // If the queue is empty (this is the first object), set its position based on the current position and offset
            // QPos.Add(obj, transform.position + (-transform.forward * qOffset));
            QPos.Add(obj, transform.position + (-transform.forward * qOffset));
        }
        else
        {
            // If the queue is not empty, set its position based on the position of the last object in the queue
            IQItem lastObject = Q[Q.Count - 2];
            QPos.Add(obj, QPos[lastObject] + (-transform.forward * qOffset));
        }
        obj.transform.position = QPos[obj];
        // insPos = QPos[obj] + (-transform.forward * qOffset);
        // insPos -= transform.forward * qOffset;
    }
    public IQItem Deque()
    {
        if (Q.Count == 0)
            return null;

        IQItem obj = Q[0];
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
    public IQItem GetFirst()
    {
        if (Q.Count == 0)
            return null;

        IQItem obj = Q[0];
        return obj;
    }
    public Vector3 GetPos(IQItem qItem)
    {
        return QPos[qItem];
    }

    // Coroutine corot;
    public void RePosition()
    {
        foreach (var item in Q)
        {
            item.GoToQPos(this);
        }
        // if (corot != null)
        // {
        //     StopCoroutine(corot);
        // }
        // if (Q.Any())
        // {

        //     corot = StartCoroutine(LocalCoroutine2());
        // }
        // IEnumerator LocalCoroutine2()
        // {
        //     float t;
        //     float time = 0;
        //     float duration = 1f;

        //     while (time < duration)
        //     {
        //         time += Time.deltaTime;
        //         t = time / duration;
        //         foreach (var item in Q)
        //         {
        //             item.transform.position = Vector3.Lerp(item.transform.position, QPos[item], t);
        //         }
        //         yield return null;
        //     }
        // }
    }

    internal void SetOrder(List<Product> products)
    {
        foreach (var item in Q)
        {
            ProductImagine Imaginary = products[Random.Range(0, products.Count)].GetRandomImage();
            Customer cust = (Customer)item;
            cust.Orders.Add(Imaginary);
        }
    }
}
