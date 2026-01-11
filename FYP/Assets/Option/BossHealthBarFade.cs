using UnityEngine;

public class BossHealthBarFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float delay = 2f;
    public float fadeDuration = 1f;

    void Start()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(FadeInAfterDelay());
    }

    private System.Collections.IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
