using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZPackage;

public class Settings : GenericSingleton<Settings>
{
    public int BulletDamage { get; set; } = 1;
    public int CoinRotationSpeed { get; set; } = 90;
    public int InitialGroundTileNumber { get; set; } = 10;
    public int LevelTriggerFurther { get; set; } = 650;
    public void ChangeLevel(string level)
    {
        print(level);
        if (int.TryParse(level, out int result))
        {
            Z.GM.Level = result;
        }

    }
    public void ChangeCoin(string coin)
    {
        if (int.TryParse(coin, out int result))
        {
            Z.GM.Coin = result;
        }

    }
}
