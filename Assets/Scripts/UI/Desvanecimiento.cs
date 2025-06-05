using UnityEngine;
using System.Collections;

public class Desvanecimiento : MonoBehaviour
{
    public CanvasGroup panelCanvasGroup;

    private void Start()
    {
       
        StartCoroutine(FadeOut(0.9f));
    }

    IEnumerator FadeOut(float duration)
    {
        float startAlpha = panelCanvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }

        panelCanvasGroup.alpha = 0f; 
    }
}
