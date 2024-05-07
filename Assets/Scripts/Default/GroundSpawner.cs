using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] Transform triggerPosition;
    [SerializeField] List<GameObject> groundTiles;
    Vector3 nextSpawnPoint;
    public int Counter { get; set; } = 0;
    int obstacleCount = 0;
    int refillerCount = 0;
    bool noMoreFloors = false;
    bool lastIndexBarunn;
    int nuhCounter = 0;

    //Start is called before the first frame update
    void Start()
    {
        triggerPosition = GameObject.Find("Baria").transform;
        SpawnTiles();
    }

    public void SpawnTiles()
    {
        noMoreFloors = false;
        refillerCount = 0;
        obstacleCount = 0;
        Counter = 0;
        SpawnFirst10Tiles();
        for (int i = 0; i < Settings.Instance.InitialGroundTileNumber; i++)
        {
            GeneralTileSpawner();
        }
    }

    public void SpawnFirst10Tiles()
    {
        SpawnTile(1,new Vector3(0,0,-10));
        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                //index 1 bol hooson zam
                SpawnTileByIndex(1);
            }
            else if (Counter % 4 == 0)
            {
                // index 3 Hondlon zusegch
                SpawnTileByIndex(3);
            }
            else
            {
                if (obstacleCount > 2)
                {
                    //3-9 hutganuud
                    int radom = Random.Range(4, 12);
                    SpawnTileByIndex(radom);

                }
                else
                {
                    int radom = Random.Range(4, groundTiles.Count);
                    SpawnTileByIndex(radom);
                }
            }
        }
    }

    public void SpawnLast5Tiles()
    {
        //index 3 buyu refiller 1 udaa gargaad 4 udaa obstacle gargana
        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(4, groundTiles.Count);
            GameObject createdFloor = SpawnTileByIndex(random);
            createdFloor.GetComponent<BoxCollider>().enabled = false;
        }
        SpawnTileByIndex(1).GetComponent<BoxCollider>().enabled = false;
    }

    public void GeneralTileSpawner()
    {
        if (!noMoreFloors) // noMorefloors == false
        {
            // index 1 bol hooson zam
            //index 2 bol obstacle zam 
            //index 4 oos hoish randomoor songogdono

            //triggerPosinionoos daraagiin zamuud hooson baina
            if (triggerPosition.position.z - 5 <= nextSpawnPoint.z)
            {
                SpawnTileByIndex(0);
                noMoreFloors = true;
            }
            else if (triggerPosition.position.z - 55 <= nextSpawnPoint.z)
            {
                SpawnLast5Tiles();
            }

            ////hanan saadnii umnu refiller gargah
            //else if (Counter % 9 == 0)
            //{
            //    SpawnTileByIndex(2);
            //}
            //10 dahi tile bolgon Hanan saad baina
            else if (Counter % 15 == 0)
            {
                //index 2 Hanan saad
                SpawnTileByIndex(2);
            }
            else
            {

                if (refillerCount > 2 /*3*/)
                {
                    int random = Random.Range(12, groundTiles.Count);
                    random = NuhSooljuul(random);
                    SpawnTileByIndex(random);
                    refillerCount = 0;
                }
                else if (nuhCounter > 2 /*4*/)
                {
                    nuhCounter = 0;
                    //4-10 hutganuud
                    int random = Random.Range(4, 12);
                    SpawnTileByIndex(random);
                }
                else
                {
                    // 4 -10 bol refiller bolon busad
                    int random = Random.Range(4, groundTiles.Count);
                    random = NuhSooljuul(random);
                    SpawnTileByIndex(random);
                }
            }
        }
    }



    private int NuhSooljuul(int random)
    {
        if (isZvvn(random) && !lastIndexBarunn)
        {
            random = Random.Range(17, 20);
        }
        else if (isBaruun(random) && lastIndexBarunn)
        {
            random = Random.Range(14, 17);
        }

        return random;
    }

    private bool IsHananSaad(int index) => index == 2;
    private bool IsRefiller(int index) => index >= 4 && index <= 12;
    private bool IsNuh(int index) => index > 13 | index == 2;
    private bool isZvvn(int index) => index >= 14 && index <= 16;
    private bool isBaruun(int index) => index >= 17 && index <= 19;


    private GameObject SpawnTileByIndex(int tileIndex)
    {
        GameObject createdFloor = SpawnTile(tileIndex,nextSpawnPoint);
        nextSpawnPoint = createdFloor.transform.GetChild(1).transform.position;
        //uusgesen tileInn toog nemegduuleh
        Counter++;
        //if (IsHananSaad(tileIndex))
        //{
        //    refillerCount = 0;
        //    obstacleCount = 0;
        //}

        if (IsRefiller(tileIndex))
        {
            refillerCount++;
        }
        else
        {
            obstacleCount++;
            if (IsNuh(tileIndex))
            {
                nuhCounter++;
                if (isZvvn(tileIndex))
                {
                    lastIndexBarunn = false;
                }
                else
                {
                    lastIndexBarunn = true;
                }
            }
        }
        return createdFloor;
    }

    private GameObject SpawnTile(int tileIndex,Vector3 position)
    {
        GameObject objectFromArray = groundTiles[tileIndex];
        GameObject createdFloor = Instantiate(objectFromArray, position, Quaternion.identity);
        return createdFloor;
    }

    public void setNextSpawnPoint(Vector3 nextSpawnPoint)
    {
        this.nextSpawnPoint = nextSpawnPoint;
    }

    int GetLastDigitOfInt(int digit)
    {
        var stringVersion = digit.ToString();
        var lastDigitStr = stringVersion.Substring(stringVersion.Length - 1);
        int lastDigitInt = int.Parse(lastDigitStr);
        return lastDigitInt;
    }
}
