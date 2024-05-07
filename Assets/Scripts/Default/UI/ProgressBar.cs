using ZPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] Image circle1;
    [SerializeField] Image circle2;
    [SerializeField] Image barOutline;
    [SerializeField] Image barFill;
    [SerializeField] Color color;
    [SerializeField] Color backgroundColor;
    private int level = 0;
    [Range(0, 1)]
    private float currentAmount = 0;
    Coroutine routine;

    private void Start()
    {
        InitColor();
        barFill.fillAmount = currentAmount;
        GameManager.Instance.GameStart += OnGameStarted;
    }
    private void OnApplicationQuit()
    {
        GameManager.Instance.GameStart -= OnGameStarted;
    }
    private void OnGameStarted(object sender, EventArgs e)
    {
        RenewProgress();
    }

    private void InitColor()
    {
        circle1.color = color;
        circle2.color = color;
        barOutline.color = color;
        barFill.color = color;
        currentLevelText.color = backgroundColor;
        nextLevelText.color = backgroundColor;
    }
    public void UpdateLevelText()
    {
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }
    public void RenewProgress()
    {
        barFill.fillAmount = 0;
        currentAmount = 0;
    }
    Coroutine updateProgress;
    public void UpdateProgress(float amount, float duration = 0.1f)
    {
        if (updateProgress != null)
        {
            StopCoroutine(updateProgress);
        }
        updateProgress = StartCoroutine(UpdateCoroutine(amount, duration));
    }

    private IEnumerator UpdateCoroutine(float amount, float duration)
    {
        float time = 0;
        var currentAmount = barFill.fillAmount;
        while (time<duration)
        {
            time += Time.deltaTime;
            barFill.fillAmount = Mathf.Lerp(currentAmount,amount,time/duration);
            yield return null;
        }
    }

    public void UpdateProgressByAmount(float amount,float duration=0.1f)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        float target = currentAmount + amount;
        routine = StartCoroutine(AddToProgress(target,duration));
    }

    private IEnumerator AddToProgress(float target, float duration)
    {
        float time = 0;
        float tempAmount = currentAmount;
        float diff = target - tempAmount;
        currentAmount = target;
        while (time<duration)
        {
            time += Time.deltaTime;
            float percent = time / duration;
            barFill.fillAmount = tempAmount + diff * percent;
            yield return null;
        }
        //if (currentAmount >= 1)
        //{
        //    LevelUp();
        //}
    }

    private void LevelUp()
    {
        UpdateLevel(level+1);
        UpdateProgressByAmount(-1f,0.2f);
    }

    public void UpdateLevel(int level)
    {
        this.level = level;
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }
}
