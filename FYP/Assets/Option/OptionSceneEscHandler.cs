using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionSceneEscHandler : MonoBehaviour
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
