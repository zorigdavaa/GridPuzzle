using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using ZPackage;
using System.Text;

public class GridController : Mb
{
    public List<Slot> Slots;
    Camera cam;
    RaycastHit hit;
    Vector3 mouseWorldPos;
    Ray ray;
    [SerializeField] Transform draggingObject;
    ISlotObj draggingObjSlot;
    [SerializeField] GameObject shooterPF;
    Vector3 dragObjOriginPos, lastDragPos;
    LayerMask roadMask;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        roadMask = LayerMask.GetMask("Road");
    }
    float countDown = 2;
    public bool dragging = false;
    // Update is called once per frame
    void Update()
    {
        // countDown -= Time.deltaTime;
        // if (countDown < 0 && !dragging)
        // {
        //     countDown = 2;
        //     InstantiatePF();
        // }

        if (IsDown)
        {
            dragging = true;
        }
        else if (IsClick && dragging)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            // Vector3 mousePos = Input.mousePosition;
            // mousePos.z = 6;
            // mouseWorldPos = cam.ScreenToWorldPoint(mousePos);
            if (draggingObject == null && Physics.Raycast(ray, out hit) && hit.transform.GetComponent<ISlotObj>() != null)
            {
                draggingObject = hit.transform;
                dragObjOriginPos = draggingObject.transform.position;
                draggingObjSlot = hit.transform.GetComponent<ISlotObj>();
                draggingObjSlot.Slot?.SetShooter(null);
                // draggingObject.GetSlot()?.SetShooter(null);
            }
            else if (draggingObject != null && Physics.Raycast(ray, out hit, 100, roadMask))
            {
                draggingObject.transform.position = hit.point + Vector3.up;
                lastDragPos = hit.point;
            }
        }
        else if (IsUp)
        {
            if (draggingObject != null)
            {
                Slot nearestSlot = null;
                float distance = 100;
                foreach (var item in Slots)
                {
                    float dis = Vector3.Distance(lastDragPos, item.transform.position);
                    if (dis < distance)
                    {
                        distance = dis;
                        nearestSlot = item;
                    }
                }
                if (distance < 1 && nearestSlot.Obj == null)
                {
                    nearestSlot.SetShooter(draggingObjSlot);
                }
                // else if (distance < 1 && nearestSlot.shooter != null && draggingObject.UpgradeAble() && nearestSlot.shooter.GetModelIndex() == draggingObject.GetModelIndex())
                else if (distance < 1 && nearestSlot.Obj != null && draggingObjSlot.IsUpgradeAble && nearestSlot.Obj.ModelIndex == draggingObjSlot.ModelIndex)
                {
                    nearestSlot.Obj.Upgrade();
                    Destroy(draggingObject.gameObject);
                }
                else
                {
                    draggingObjSlot.Slot?.SetShooter(draggingObjSlot);
                }
                draggingObject = null;
                draggingObjSlot = null;
            }

            dragging = false;
        }
    }

    private void InstantiatePF()
    {
        Slot firstFreeSlot = Slots.Where(x => x.Obj == null).FirstOrDefault();
        if (firstFreeSlot)
        {
            GameObject shooter = Instantiate(shooterPF, firstFreeSlot.transform.position, Quaternion.identity);
            firstFreeSlot.SetShooter(shooter.GetComponent<ISlotObj>());
        }
    }
    public GameObject InstantiatePF(Slot slot)
    {
        GameObject shooter = Instantiate(shooterPF, slot.transform.position, Quaternion.identity);
        slot.SetShooter(shooter.GetComponent<ISlotObj>());
        return shooter;
    }
    public void SaveSlots()
    {
        List<SaveSlot> saveSlots = new List<SaveSlot>();
        foreach (var item in Slots)
        {
            // SaveSlot ss = new SaveSlot((item.shooter != null), item.shooter.GetModelIndex(), item.shooter.dam);
            if (item.Obj != null)
            {
                saveSlots.Add(new SaveSlot(true, item.Obj.ModelIndex, 0));
            }
            else
            {
                saveSlots.Add(new SaveSlot());
            }
        }
        StringBuilder builder = new StringBuilder();
        foreach (var item in saveSlots)
        {
            builder.Append(item.hasShooter ? 1 : 0);
            // builder.Append(",");

            builder.Append(item.curModelIndex);
            // builder.Append(",");

            builder.Append(item.dam);
            // builder.Append(",");

        }
        PlayerPrefs.SetString("list", builder.ToString());
        // print(builder);
    }
    public void GetSaveSlots()
    {
        string saveString = PlayerPrefs.GetString("list");
        print(saveString);
        List<SaveSlot> saveSlots = new List<SaveSlot>();
        int stringIndex = 0;
        for (int i = 0; i < Slots.Count; i++)
        {
            saveSlots.Add(new SaveSlot(int.Parse(saveString[stringIndex].ToString()) == 1, int.Parse(saveString[stringIndex + 1].ToString()), int.Parse(saveString[stringIndex + 2].ToString())));
            // saveSlots[i].hasShooter = int.Parse(saveString[stringIndex].ToString()) == 1;
            // print(int.Parse(saveString[stringIndex].ToString()));
            // saveSlots[i].curModelIndex = int.Parse(saveString[stringIndex + 1].ToString());
            // saveSlots[i].dam = int.Parse(saveString[stringIndex + 2].ToString());
            stringIndex += 3;
            if (saveSlots[i].hasShooter)
            {
                // print("dd");
                GameObject shooter = InstantiatePF(Slots[i]);
                for (int j = 0; j < saveSlots[i].curModelIndex; j++)
                {
                    shooter.GetComponent<ISlotObj>().Upgrade();
                }
            }
        }

    }

    internal void ReleaseSlotObjs()
    {
        throw new NotImplementedException();
    }
}
