using UnityEngine;

public class MenuButtonHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadScene("SampleScene");
    }

    public void ContinueGame()
    {
        SceneLoader.LoadScene("SampleScene");
    }

    public void OpenOptions()
    {
        SceneLoader.LoadScene("OptionScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
