using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public ISlotObj Obj;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetShooter(ISlotObj slotObj)
    {
        Obj = slotObj;
        if (slotObj != null)
        {
            Obj.Slot = this;
            Obj.transform.position = transform.position;
            // shooter.SetSlot(this);
            // shooter.transform.position = transform.position;
        }
    }

}
