using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZPackage;

public class ZButton : Button
{
    public UnityEvent<SelectionStateWrap> buttonStateChanged;
    public Color color;
    [SerializeField]
    private ButtonClickedEvent m_OnDown = new ButtonClickedEvent();
    protected SelectionState prevState;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        if (state != prevState)
        {
            prevState = state;
            // switch (state)
            // {
            //     case SelectionState.Normal: Debug.Log("Normal"); break;
            //     case SelectionState.Disabled: Debug.Log("Normal"); break;
            //     default: break;
            // }
            SelectionStateWrap wrapped = ToPublicEnum((int)state);
            buttonStateChanged?.Invoke(wrapped);
        }
    }
    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        base.OnPointerDown(pointerEventData);
        m_OnDown?.Invoke();
    }
    public SelectionStateWrap ToPublicEnum(int value)
    {
        // Convert internal enum to a public enum
        return (SelectionStateWrap)value;
    }


}

public enum SelectionStateWrap
{
    /// <summary>
    /// The UI object can be selected.
    /// </summary>
    Normal,

    /// <summary>
    /// The UI object is highlighted.
    /// </summary>
    Highlighted,

    /// <summary>
    /// The UI object is pressed.
    /// </summary>
    Pressed,

    /// <summary>
    /// The UI object is selected
    /// </summary>
    Selected,

    /// <summary>
    /// The UI object cannot be selected.
    /// </summary>
    Disabled,
}
