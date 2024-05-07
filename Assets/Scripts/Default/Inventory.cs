using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int Capacity = 20;
    public bool IsCapacityFull => Objects.Count >= Capacity;
    [SerializeField] List<GameObject> Objects = new List<GameObject>();
    public EventHandler OnInventoryDepleted;
    Vector3 topPos = Vector3.zero;
    [SerializeField] GameObject FullText;
    Vector3 GetTopPos()
    {
        // Vector3 tempPos = topPos;
        // if (increase)
        // {
        //     topPos += Vector3.up;
        // }
        // return tempPos;

        return Vector3.up * 0.5f * Objects.Count;
    }

    internal GameObject GetItemByTag(string v)
    {
        foreach (var item in Objects)
        {
            if (item.CompareTag(v))
            {
                Objects.Remove(item);
                return item;
            }
        }
        return null;
    }

    Vector3 GetTopPos(int index)
    {
        return Vector3.up * index;
    }
    public int Count => Objects.Count;
    public List<GameObject> GetObjects => Objects;
    // public void RefreshTopPos()
    // {
    //     topPos = Vector3.zero;
    //     for (int i = 0; i < Objects.Count; i++)
    //     {
    //         topPos += Vector3.up;
    //     }
    // }
    public void AddInventory(GameObject inventory)
    {
        if (IsCapacityFull)
        {
            return;
        }
        // if (inventory.GetComponent<Stack>())
        // {
        //     inventory.GetComponent<Stack>().SetInventory(this);
        //     inventory.GetComponent<Stack>().PlayParticle();
        // }

        if (Objects.Contains(inventory))
        {
            return;
        }
        inventory.transform.SetParent(transform);
        inventory.transform.localPosition = GetTopPos();
        inventory.transform.rotation = transform.rotation;
        Objects.Add(inventory);
        // inventory.GetComponent<Collect>().HideCollider();
        if (IsCapacityFull)
        {
            // FullText.SetActive(true);
        }
    }

    public void RemoveFromList(GameObject inventory, bool refresh = false)
    {
        Objects.Remove(inventory);
        if (refresh)
        {
            RefreshInventoryPositions();
        }
    }
    public void RemoveFirst(bool refresh = false)
    {
        if (!HasItem())
        {
            return;
        }
        GameObject firstOne = Objects[0];
        // Ene Ironii orond oor heregtei
        // firstOne.GetComponent<Stack>().SetFree();
        Objects.Remove(firstOne);
        if (refresh)
        {
            RefreshInventoryPositions();
        }
    }
    public bool Contains(GameObject obj)
    {
        return Objects.Contains(obj);
    }

    internal int DropAll()
    {
        int count = Objects.Count;
        // foreach (var item in Objects)
        // {
        //     item.GetComponent<Collect>().SetFree();
        //     item.GetComponent<Collect>().BecomeTrigger();
        // }
        Objects.RemoveAll(x => x);
        // FullText.SetActive(false);
        return count;
    }

    public void RemoveLast(bool refresh = false)
    {
        if (!HasItem())
        {
            return;
        }
        GameObject lastOne = Objects[Objects.Count - 1];
        // Ene Ironii orond oor heregtei
        // lastOne.GetComponent<Stack>().SetFree();
        Objects.Remove(lastOne);
        if (refresh)
        {
            RefreshInventoryPositions();
        }
    }
    public bool HasItem()
    {
        bool response = Objects.Count > 0;

        return response;
    }

    public void Divide(int amount)
    {
        int currentCount = Objects.Count;
        int dividedCount = currentCount / amount;

        throw new NotImplementedException();
    }

    public GameObject GetLastItem()
    {
        GameObject obj = Objects[Objects.Count - 1];
        Objects.RemoveAt(Objects.Count - 1);
        obj.transform.SetParent(null);
        if (Objects.Count <= 0)
        {
            OnInventoryDepleted?.Invoke(this, EventArgs.Empty);
        }
        // FullText.SetActive(false);
        return obj;
    }
    public GameObject GetFirstItem()
    {
        GameObject obj = Objects[0];
        Objects.RemoveAt(0);
        obj.transform.SetParent(null);
        //BecomePlayer
        // Objects[0].GetComponent<Stack>().BecomePlayer();
        RefreshInventoryPositions();
        return obj;
    }
    public void RefreshInventoryPositions()
    {
        StopAllCoroutines();
        // StartCoroutine(RefreshCor());
        // Vector3 pos = Vector3.zero;
        RefreshCor();
        void RefreshCor()
        {
            // yield return new WaitForSeconds(0.7f);
            int index = 0;
            foreach (var item in Objects)
            {
                // if (item.transform.localPosition != GetTopPos())
                // {
                StartCoroutine(GotoPos(item.transform, GetTopPos(index)));
                index++;
                // }
                // pos = pos + Vector3.up * 0.3f;
            }
        }
    }



    IEnumerator GotoPos(Transform item, Vector3 pos)
    {
        float time = 0;
        float duration = 0.5f;
        while (time < duration)
        {
            if (item.transform.localPosition == pos)
            {
                yield break;
            }
            time += Time.deltaTime;
            item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, pos, time / duration);
            yield return null;
        }
        item.transform.localPosition = pos;
    }
}
