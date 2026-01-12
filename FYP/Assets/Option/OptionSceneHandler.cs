using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionSceneHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!string.IsNullOrEmpty(SceneTracker.PreviousScene))
            {
                SceneManager.LoadScene(SceneTracker.PreviousScene);
            }
        }
    }
}
