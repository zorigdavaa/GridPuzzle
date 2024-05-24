using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PuzzleSlotDoor : PuzzleSlot
{
    [SerializeField] PuzzleSlot leftObj;
    public Bot botPf;
    [SerializeField] int _insCount;
    public int InsCount
    {
        get { return _insCount; }
        set
        {
            _insCount = value;
            uiCounter.text = value.ToString();
        }
    }

    public int DefaultInsCount = 3;
    [SerializeField] List<IPuzzleObj> InsObjs;
    IPuzzleObj Obj;
    public TextMeshPro uiCounter;

    // Start is called before the first frame update
    public override void Init()
    {
        GetComponent<GridNode>().Blocked = true;
        InsCount = DefaultInsCount;
        leftObj = GetComponent<GridNode>().GetLeftObj();
        leftObj.OnBotNull += OnLeftBotNull;

    }


    private void OnLeftBotNull(object sender, EventArgs e)
    {
        // print("Null ");
        StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            if (InsCount > 0 && leftObj.GetPuzzleObj() == null)
            {
                Obj = InsObjs[InsCount - 1];
                InsCount--;
                // GetComponent<GridNode>().OwnGrid.InstantiateBot(Color, leftObj.GetComponent<PuzzleSlot>());
                IPuzzleObj insBot = FindObjectOfType<PuzzleController>().InsPuzzleObj(this, Obj.gameObject);
                List<Vector3> pathf = new List<Vector3>() { transform.position, leftObj.transform.position };
                insBot.GotoSlot(leftObj.GetComponent<GridNode>(), pathf);
                // A.BusController.GetComponent<PuzzleController>().CheckAllBotPaths();


            }
            // else
            // {
            //     leftObj.OnBotNull -= OnLeftBotNull;
            // }
        }
    }
    public override void Refresh()
    {
        print("Refershed Door");
        if (leftObj == null)
        {
            leftObj = GetComponent<GridNode>().GetLeftObj();
        }
        // if (leftObj.OnBotNull.GetInvocationList().Count() == 0)
        // {
        //     leftObj.OnBotNull += OnLeftBotNull;
        // }
        // leftObj.OnBotNull += OnLeftBotNull;
        InsCount = DefaultInsCount;
    }
}
