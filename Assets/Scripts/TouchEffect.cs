using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    private Action nextAction;
    private float fadeTimer = 0.2f;
    
    private void Start()
    {
        nextAction = FadeOut;
        SetPlay();
    }

    public void SetPlay()
    {
        FadeIn(fadeTimer, nextAction);
    }

    public void Next()
    {
        Destroy(this.gameObject);
    }

    public void FadeIn(float fadeOutTime, Action nextEvent = null)
    {
        StartCoroutine(CoFadeIn(fadeOutTime, nextEvent));
    }
    
    public void FadeOut()
    {
        StartCoroutine(CoFadeOut());
    }

    // 투명 -> 불투명
    IEnumerator CoFadeIn(float fadeOutTime, Action nextEvent = null)
    {
        Material sr = this.gameObject.GetComponent<Renderer>().material;
        Color tempColor = sr.color;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            sr.color = tempColor;
            if (tempColor.a > 1f) tempColor.a = 1f;
            yield return null;
        }

        sr.color = tempColor;
        if (nextEvent != null) nextEvent();
    }

    // 불투명 -> 투명
    IEnumerator CoFadeOut()
    {
        Material sr = this.gameObject.GetComponent<Renderer>().material;
        Color tempColor = sr.color;

        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeTimer;
            sr.color = tempColor;
            if (tempColor.a < 0f) tempColor.a = 0f;
            yield return null;
        }

        sr.color = tempColor;
    }
}
