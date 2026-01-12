using UnityEngine;
using UnityEngine.SceneManagement;
using DuloGames.UI;

public class GameMenuButtonHandler : MonoBehaviour
{
    public void ResumeGame()
    {
        if (UIWindowManager.Instance != null)
        {
            UIWindowManager.Instance.ResumeGame();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void LogoutToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void OpenOptions()
    {
        Time.timeScale = 1f;
        SceneTracker.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("OptionScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
