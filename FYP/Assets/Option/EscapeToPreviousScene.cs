using UnityEngine;

public class EscapeToPreviousScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.LoadPreviousScene();
        }
    }
}
