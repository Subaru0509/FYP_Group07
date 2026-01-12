using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private float fadeDuration = 2f;
    private bool isFading = false;
    private float timer = 0f;

    public void ShowVictory()
    {
        if (victoryPanel == null) return;
        victoryPanel.gameObject.SetActive(true);
        victoryPanel.alpha = 0f;
        isFading = true;
        timer = 0f;
    }

    void Update()
    {
        if (!isFading) return;
        timer += Time.deltaTime;
        victoryPanel.alpha = Mathf.Clamp01(timer / fadeDuration);
        if (victoryPanel.alpha >= 1f) isFading = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }
}
