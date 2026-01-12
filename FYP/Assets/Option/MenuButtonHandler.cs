using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuButtonHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
