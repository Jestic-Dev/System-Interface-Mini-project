using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixRoomFader : MonoBehaviour
{

    public Image blurEffect;
    public float blurAmount;
    public float blurTime;

    public float teleDarknessTime;
    public float teleInBlurTime;
    public float teleOutBlurTime;

    private Coroutine currentFade;

    private void CancelCurrentFade()
    {
        if(currentFade != null)
        {
            StopCoroutine(currentFade);
        }
    }

    public void FadeEnvironment()
    {
        CancelCurrentFade();
        currentFade = StartCoroutine(FadingEnvironment(blurAmount));
    }

    public void UnfadeEnvironment()
    {
        CancelCurrentFade();
        currentFade = StartCoroutine(FadingEnvironment(0));
    }

    private IEnumerator FadingEnvironment(float fadeToValue)
    {
        float fadingTime = 0;
        while (blurEffect.color.a != fadeToValue)
        {
            float newAlpha = Mathf.Lerp(blurEffect.color.a, fadeToValue, fadingTime);
            blurEffect.color = new Color(blurEffect.color.r, blurEffect.color.g, blurEffect.color.b, newAlpha);
            fadingTime += Time.deltaTime / blurTime;
            yield return null;
        }
    }

    public void FadeTeleport(Transform toTeleport, Vector3 teleToPosition)
    {
        CancelCurrentFade();
        currentFade = StartCoroutine(FadingTeleport(toTeleport, teleToPosition));
    }

    private IEnumerator FadingTeleport(Transform toTeleport, Vector3 teleToPosition)
    {
        float fadingTime = 0;
        float startAlpha = blurEffect.color.a;
        while (blurEffect.color.a < 1)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 1, fadingTime);
            blurEffect.color = new Color(blurEffect.color.r, blurEffect.color.g, blurEffect.color.b, newAlpha);
            fadingTime += Time.deltaTime / teleInBlurTime;
            yield return null;
        }
        Debug.Log("Finished fading to dark");

        yield return new WaitForSeconds(teleDarknessTime);
        toTeleport.position = teleToPosition;

        fadingTime = 0;
        while (blurEffect.color.a > 0)
        {
            float newAlpha = Mathf.Lerp(1, 0, fadingTime);
            blurEffect.color = new Color(blurEffect.color.r, blurEffect.color.g, blurEffect.color.b, newAlpha);
            fadingTime += Time.deltaTime / teleOutBlurTime;
            yield return null;
        }
        Debug.Log("Finished unfading");
    }
}
