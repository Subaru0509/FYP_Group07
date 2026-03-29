using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageOptionDropdown_UI : MonoBehaviour
{
    [SerializeField] private string optionName;
    [SerializeField] private TMP_Dropdown optionDropdown;

    //[Header("Chinese Language")]
    //[SerializeField] private TMP_FontAsset chineseFont;

    //[Header("English Language")]
    //[SerializeField] private TMP_FontAsset englishFont;

    //private TextMeshProUGUI[] texts;

    private void Start()
    {
        //texts = FindObjectsOfType<TextMeshProUGUI>();
        optionDropdown.value = LanguageManager.instance.localeID;
    }

    public void SetTextLanguage()
    {
        StartCoroutine(SetLocale_Coroutine(optionDropdown.value));
    }

    //0 for English, 1 for Chinese
    private IEnumerator SetLocale_Coroutine(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;

        LanguageManager.instance.localeID = _localeID;
        LanguageManager.instance.SetTextLanguageByLocaleID(_localeID);

        KeyBindManager.instance?.UpdateKeybindListLanguage();

        //unity has bug, null check has to be in if-statement type here
        //otherwise using ?-type will not work
        if (SkillPanel_InGame_UI.instance != null)
        {
            SkillPanel_InGame_UI.instance.UpdateAllSkillIconTexts();
        }

        //yield return new WaitUntil(SetTextFont);
    }

    //private bool SetTextFont()
    //{
    //    if (LanguageManager.instance.localeID == 1)
    //    {
    //        foreach (var text in texts)
    //        {
    //            text.font = chineseFont;
    //        }

    //        return true;
    //    }
    //    else if (LanguageManager.instance.localeID == 0)
    //    {
    //        foreach (var text in texts)
    //        {
    //            text.font = englishFont;
    //        }

    //        return true;
    //    }

    //    return false;
    //}
}
