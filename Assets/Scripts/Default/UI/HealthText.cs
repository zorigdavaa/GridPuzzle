using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField] TextMeshPro healthText;
    // Start is called before the first frame update
    private void Awake()
    {
        healthText = GetComponent<TextMeshPro>();
    }

    public void RefreshText(string incomingText)
    {
        healthText.text = incomingText;
    }
}
