using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options_MainMenu_UI : MonoBehaviour
{
    public void SaveAndReturnToTitle()
    {
        StartCoroutine(SaveSettingsAndReturnToTitle_Coroutine());
    }

    private IEnumerator SaveSettingsAndReturnToTitle_Coroutine()
    {
        SaveManager.instance.SaveSettings();

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("MainMenu");
    }

}
