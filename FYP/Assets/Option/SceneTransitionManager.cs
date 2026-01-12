using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransitionManager
{
    public static void TransitionTo(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
}
