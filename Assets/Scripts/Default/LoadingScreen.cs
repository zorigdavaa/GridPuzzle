using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    TMP_Text loadingText;

    void Awake()
    {
        loadingText = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        float textCol;
        textCol = Mathf.PingPong(Time.time * 1, 1f);
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, textCol);
    }
}
