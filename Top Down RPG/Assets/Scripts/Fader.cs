using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public SpriteRenderer render;
    public bool automaticTransition;
    public bool fadeOutOnly;
    public bool fadeInOnly;
    public float startFadeOutCountdown;
    public float startFadeInCountdown;
    float fadeOutDuration;
    float fadeInDuration;

    void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        fadeOutDuration = 0.95f;
        fadeInDuration = 1.05f;

        if (automaticTransition)
        {
            if (fadeOutOnly)
                StartCoroutine(FadeOutOnly(fadeOutDuration));
            else if (fadeInOnly)
                StartCoroutine(FadeInOnly(fadeInDuration));
            else
                StartCoroutine(FadeOutAndIn(fadeOutDuration, fadeInDuration));
        }
    }

    public IEnumerator FadeOutOnly(float fadeOutDuration)
    {
        yield return new WaitForSeconds(startFadeOutCountdown);

        for (float alpha = fadeOutDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeInOnly(float fadeInDuration)
    {
        UpdateFrame(0);
        yield return new WaitForSeconds(startFadeInCountdown);

        for (float alpha = 0.05f; alpha <= fadeInDuration; alpha += 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeOutAndIn(float fadeOutDuradion, float fadeInDuration)
    {
        yield return new WaitForSeconds(startFadeOutCountdown);

        for (float alpha = fadeOutDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(startFadeInCountdown - (startFadeOutCountdown + fadeOutDuration));
        for (float alpha = 0.05f; alpha <= fadeInDuration; alpha += 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void UpdateFrame(float f)
    {
        Color c = render.material.color;
        c.a = f;
        render.material.color = c;
    }

}
