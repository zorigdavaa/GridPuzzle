using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] TextMeshPro threeDText;
    public void SetUiText(string text)
    {
        textField.text = text;
    }
    public void SetThreeDText(string text)
    {
        threeDText.text = text;
    }
}
