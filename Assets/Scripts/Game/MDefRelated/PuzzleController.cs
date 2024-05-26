using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using ZPackage;
using ZPackage.Helper;
using Random = UnityEngine.Random;

public class PuzzleController : Mb
{
    Camera cam;
    Ray ray;
    List<PuzzleSlot> chosenSlots = new List<PuzzleSlot>();
    public List<PuzzleSlot> Slots;
    public bool isPuzzling = false;
    public GridMono grid;
    QueueManager queueManager;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        queueManager = FindObjectOfType<QueueManager>();
        Init();
        GameManager.Instance.GamePlay += OnGamePlay;
    }
    // [ContextMenu("Placement")]
    public void Init()
    {
        foreach (var item in Slots)
        {
            bool isChosen = item.GetComponent<GridNode>().Y == grid.GetHeight() - 1;
            bool isHideSlot = item.GetComponent<GridNode>().Y == grid.GetHeight() - 2;
            item.isChosenSlot = isChosen;
            // Color color = isChosen ? Color.white : new Color32(202, 202, 202, 255);
            if (isChosen)
            {
                chosenSlots.Add(item);
                item.SetColor(Color.white);
            }
            else
            {
                item.SetColor(new Color32(202, 202, 202, 255));
            }
            if (isHideSlot)
            {
                item.gameObject.SetActive(false);
                item.GetComponent<GridNode>().Blocked = false;
            }
        }
        ShuffledSlots = Slots.Where(x => x.gameObject.activeSelf && !x.isChosenSlot).ToList();
        ShuffledSlots.Shuffle();
        RefillPuzzle();
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        StartPuzlle();
    }

    List<PuzzleSlot> FreeGoNodes;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Refresh();
        }
        if (isPuzzling && IsDown)
        {
            ray = cam.ScreenPointToRay(MP);
            // bool isHit = Physics.Raycast(ray, out RaycastHit hit, 20, layermask);
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, 200);
            IPuzzleObj selectedObj = null;
            if (isHit)
            {
                if (hit.transform.GetComponent<IPuzzleObj>() != null)
                {
                    selectedObj = hit.transform.GetComponent<IPuzzleObj>();
                }
                // else// onoltiig oirhon onosonch saijruulna 
                // {
                //     // print(hit.transform.gameObject.name);
                //     Collider[] aroundObjs = Physics.OverlapSphere(hit.point, 0.5f);
                //     // print(aroundObjs.Length);
                //     float minDistance = Mathf.Infinity;
                //     foreach (var obj in aroundObjs)
                //     {
                //         Bot bot = obj.transform.GetComponent<Bot>();
                //         if (bot != null)
                //         {
                //             float distance = Vector3.Distance(hit.point, bot.transform.position);
                //             if (distance < minDistance)
                //             {
                //                 selectedObj = bot;
                //                 minDistance = distance;
                //             }
                //         }
                //     }
                // }
            }

            // print("Down and hit was " + isHit);
            if (selectedObj != null && selectedObj.currentSlot?.GridNode)
            // if (hitObjs.Length > 0 && selectedObj && selectedObj.currentSlot?.GridNode)
            {
                selectedObj.Clicked(this, EventArgs.Empty);
                GridNode botNode = selectedObj.currentSlot.GridNode;
                FreeGoNodes = chosenSlots.Where(x => x.GetPuzzleObj() == null && !x.GetComponent<GridNode>().Blocked).ToList();
                GridNode goNode = null;
                if (FreeGoNodes.Count > 0)
                {
                    goNode = FreeGoNodes?.First().GridNode;
                }
                // print(goNode + " go Nodes");
                // print(!selectedObj.currentSlot.isChosenSlot + " 2nd");
                if (goNode != null && !selectedObj.currentSlot.isChosenSlot)
                {
                    List<Vector3> paths = grid.FindPath(botNode, goNode);
                    // print("Path Count is  " + paths.Count);
                    // print(goNode.name + " Go Node");
                    if (paths.Count > 0)
                    {
                        // print(paths.Count + " pathc");
                        selectedObj.currentSlot.SetBot(null);
                        Action afterAction = () =>
                        {
                            selectedObj.ChosenState = true;
                            FreeGoNodes = chosenSlots.Where(x => x.GetPuzzleObj() == null && !x.GetComponent<GridNode>().Blocked).ToList();
                            Debug.Log(FreeGoNodes.Count);
                            if (FreeGoNodes.Count == 0)
                            {
                                // bool SameColor = false;
                                // List<Color> FirsColorsQ = queueManager.Queues.Select(x => x.GetFirst().GetColor()).ToList();
                                // List<Color> ChSlotColors = chosenSlots.Select(x => x.GetPuzzleObj().GetColor()).ToList();
                                // foreach (var item in ChSlotColors)
                                // {
                                //     foreach (var QFirstColor in FirsColorsQ)
                                //     {
                                //         if (item == QFirstColor)
                                //         {
                                //             SameColor = true;
                                //             break;
                                //         }
                                //     }
                                //     if (SameColor)
                                //     {
                                //         break;
                                //     }
                                // }
                                // if (!SameColor)
                                // {
                                // }
                                Z.GM.GameOver(this, EventArgs.Empty);
                            }
                            else
                            {
                                Merge();
                            }
                        };
                        if (FreeGoNodes.Count <= 2) //when Last
                        {

                        }

                        selectedObj.GotoSlot(goNode, paths, afterAction);
                    }

                }

            }

            CheckAllBotPaths();
        }
    }

    public bool Merge()
    {
        List<IMergeAble> ChosenIngs = new List<IMergeAble>();
        ChosenIngs = chosenSlots.Where(x => x.Bot != null).Select(x => (IMergeAble)x.Bot).ToList();
        // List<Ingredient> ToBeMergeobjs = new List<Ingredient>();
        print("Count is " + ChosenIngs.Count);
        if (ChosenIngs.Count > 1)
        {
            MergeManager Mergemanager = FindObjectOfType<MergeManager>();
            for (int i = 0; i < ChosenIngs.Count; i++)
            {
                for (int j = i + 1; j < ChosenIngs.Count; j++)
                {
                    // if (i == j)
                    // {
                    //     continue;
                    // }
                    IMergeAble a = ChosenIngs[i];
                    IMergeAble b = ChosenIngs[j];
                    // print("MergeAble is " + Mergemanager.IsMergeAble(a, b));
                    if (Mergemanager.IsMergeAble(a, b))
                    {
                        // ToBeMergeobjs.Add(a);
                        // ToBeMergeobjs.Add(b);
                        Mergemanager.Merge(a, b);
                        return true;
                        // break;
                    }
                }

            }
        }
        return false;

    }//[0,1][0,2][0,3][1,0][1,2][1,3][2,0] etc

    GridNode checkCanGoChosenNode;
    //
    public void CheckAllBotPaths()
    {
        if (Slots != null)
        {
            foreach (PuzzleSlot slot in Slots)
            {
                IPuzzleObj bot = slot.GetPuzzleObj();
                if (bot != null && !bot.currentSlot.isChosenSlot)
                {
                    GridNode botNode = bot.currentSlot.GridNode;
                    List<Vector3> paths = grid.FindPath(botNode, checkCanGoChosenNode);
                    // print(paths.Count + " Paths");
                    if (paths.Count > 0)
                    {
                        // bot.ShowIsClickable(true);
                    }

                }
            }
        }
    }



    public void Refresh()
    {
        List<PuzzleSlot> FreeSlots = Slots.Where(x => x.GetPuzzleObj() != null && !x.isChosenSlot).ToList();
        List<PuzzleSlot> FreeGoNodes = chosenSlots.Where(x => x.GetPuzzleObj() == null && !x.GetComponent<GridNode>().Blocked).ToList();
        if (isPuzzling && (FreeGoNodes.Count < 1 || FreeSlots.Count == 0))
        {
            StartCoroutine(LocalCoroutine());
            IEnumerator LocalCoroutine()
            {
                yield return new WaitForSeconds(1);
                HardRefresh();
            }
        }
    }

    public void HardRefresh()
    {
        // currentBusStop.Grid.RefillPuzzle(true);
        CheckAllBotPaths();
    }


    internal void StartPuzlle()
    {
        // chosenSlots = grid.GetChosenSlots();
        // Slots = grid.Slots;
        checkCanGoChosenNode = grid.GetHidedSecondGrid();
        isPuzzling = true;

        // CheckAllBotPaths();

    }

    internal void StopPuzzle()
    {
        isPuzzling = false;
        // if (currentBusStop)
        // {

        //     currentBusStop.ShowPuzzle(false);
        // }
    }

    internal List<IPuzzleObj> GetClickAbleBots()
    {
        List<IPuzzleObj> SeatchBots = Slots.Where(x => x.GetPuzzleObj() != null).Select(x => x.Bot).ToList();
        return Slots.Where(x =>
        !x.isChosenSlot &&
        x.gameObject.activeSelf &&
        !x.GetComponent<GridNode>().Blocked &&
        x.GetPuzzleObj() != null).Select(x => x.Bot).ToList();
    }
    [SerializeField] GameObject[] BotPfs;
    List<PuzzleSlot> ShuffledSlots;
    internal void RefillPuzzle()
    {

        int index = 0;
        // print(ShuffledSlots.Count + " Count is");
        foreach (var item in ShuffledSlots)
        {
            if (item.GetPuzzleObj() != null)
            {
                Destroy(item.GetPuzzleObj().gameObject);
            }
            item.SetBot(null, true);
            GridNode node = item.GetComponent<GridNode>();
            if (item.GetPuzzleObj() == null && !item.isChosenSlot && !node.Blocked && item is not PuzzleSlotDoor)
            {
                IPuzzleObj obj = InsPuzzleObj(item, BotPfs[Random.Range(0, BotPfs.Length)]);
                index++;
            }
            item.Refresh();
        }
    }



    public IPuzzleObj InsPuzzleObj(PuzzleSlot item, GameObject botPf)
    {
        // Quaternion rot = Quaternion.LookRotation(-transform.right);
        Quaternion rot = Quaternion.identity;
        IPuzzleObj obj = Instantiate(botPf, item.transform.position, rot, transform).GetComponent<IPuzzleObj>();
        item.SetBot(obj);

        obj.transform.localScale = Vector3.one * 0.9f;
        return obj;
    }

}

