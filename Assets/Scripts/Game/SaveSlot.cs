using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSlot
{
    public bool hasShooter;
    public int curModelIndex;
    public int dam;
    public SaveSlot(bool _hasShooter, int curModelIndx, int _dam)
    {
        hasShooter = _hasShooter;
        curModelIndex = curModelIndx;
        dam = _dam;
    }
    public SaveSlot()
    {
        
    }

}
