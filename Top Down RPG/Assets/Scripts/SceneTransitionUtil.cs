using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUtil : MonoBehaviour
{
    public SpriteRenderer render;
    public bool automaticTransition;
    public bool fadeInOnly;
    public bool fadeOutOnly;
    public float startFadeInCountdown;
    public float startFadeOutCountdown;
    float fadeInDuration;
    float fadeOutDuration;

    void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        fadeInDuration = 0.95f;
        fadeOutDuration = 1.05f;

        if (automaticTransition)
        {
            if (fadeInOnly)
                StartCoroutine(FadeInOnly(fadeInDuration));
            else if (fadeOutOnly)
                StartCoroutine(FadeOutOnly(fadeOutDuration));
            else
                StartCoroutine(FadeInAndOut(fadeInDuration, fadeOutDuration));
        }
    }

    public IEnumerator FadeInOnly(float fadeInDuration)
    {
        yield return new WaitForSeconds(startFadeInCountdown);

        for (float alpha = fadeInDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeOutOnly(float fadeOutDuration)
    {
        UpdateFrame(0);
        yield return new WaitForSeconds(startFadeOutCountdown);

        for (float alpha = 0.05f; alpha <= fadeOutDuration; alpha += 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeInAndOut(float fadeInDuradion, float fadeOutDuration)
    {
        yield return new WaitForSeconds(startFadeInCountdown);

        for (float alpha = fadeInDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(startFadeOutCountdown - (startFadeInCountdown + fadeInDuration));
        for (float alpha = 0.05f; alpha <= fadeOutDuration; alpha += 0.05f)
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
