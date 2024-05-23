using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class PuzzleSlot : MonoBehaviour
{
    // public PuzzleSlot gridPos;
    // Grid<PuzzleSlot> grid;
    public Bot Bot;
    public bool isChosenSlot = false;
    public bool isColorHidden = false;
    public Color Color = Color.black;

    public GridNode GridNode;
    public EventHandler OnBotNull;
    internal Bot GetBot()
    {
        return Bot;
    }

    internal virtual void SetBot(Bot bot, bool instantPlacement = true)
    {

        Bot = bot;
        if (bot)
        {
            Bot.currentSlot = this;
            if (instantPlacement)
            {
                Bot.transform.position = transform.position;
            }
            // Bot.GetComponent<Collider>().size = new Vector3(0.6f, 2, 0.6f);
        }
        else
        {
            // if (Caller != String.Empty)
            // {
            //     print("Caller was " + Caller);
            // }
            OnBotNull?.Invoke(this, EventArgs.Empty);
        }
    }
    public void SetColor(Color color)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }
    // internal void SetGridPos(PuzzleSlot gemGridPos, Grid<PuzzleSlot> grid)
    // {
    //     // gridPos = gemGridPos;
    //     // this.grid = grid;
    //     // StartCoroutine(LocalCoroutine(grid.GetLocalPLacement(gridPos.pos.x, gridPos.pos.y)));
    //     // IEnumerator LocalCoroutine(Vector3 toPos)
    //     // {
    //     //     float t = 0;
    //     //     float time = 0;
    //     //     float duration = 0.5f;
    //     //     Vector3 initialPosition = transform.localPosition;
    //     //     while (time < duration)
    //     //     {
    //     //         time += Time.deltaTime;
    //     //         t = time / duration;
    //     //         transform.localPosition = Vector3.Lerp(initialPosition, toPos, t);
    //     //         yield return null;
    //     //     }
    //     // }
    // }




    internal Color GetColor()
    {
        return Color;
    }
    public virtual void Init()
    {

    }

    public virtual void Refresh()
    {

    }

    //void Update()
    //{
    //    if (isChosenSlot)
    //    {
    //        if (Bot)
    //        {
    //            Bot.TurnOnOutline(true);
    //        }
    //    }
    //}
}