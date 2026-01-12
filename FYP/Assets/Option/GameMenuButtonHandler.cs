using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuButtonHandler : MonoBehaviour
{
    public void ResumeGame()
    {
        Time.timeScale = 1f;
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
