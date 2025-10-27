using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); 
    }

    
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }
}
