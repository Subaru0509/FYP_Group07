using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options_UI : MonoBehaviour
{
    [SerializeField] private FadeScreen_UI fadeScreen;
    public void SaveAndReturnToTitle()
    {
        StartCoroutine(SaveAndReturnToTitle_Coroutine());
    }

    private IEnumerator SaveAndReturnToTitle_Coroutine()
    {
        SaveManager.instance.SaveGame();
        GameManager.instance.PauseGame(false);
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("MainMenu");
    }
}
