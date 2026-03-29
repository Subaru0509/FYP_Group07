using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillIconText_InGame_UI : MonoBehaviour
{
    [SerializeField] private string skillName;

    public void UpdateSkillIconText()
    {
        if (KeyBindManager.instance.keybindsDictionary.TryGetValue(skillName, out KeyCode keybind))
        {
            GetComponent<TextMeshProUGUI>().text = UniformKeybindNameForInGameUI(keybind.ToString());

            //chinese needs to decrease font size a bit
            if (LanguageManager.instance.localeID == 1)
            {
                GetComponent<TextMeshProUGUI>().fontSize = 22;
            }
            //english
            else if (LanguageManager.instance.localeID == 0)
            {
                GetComponent<TextMeshProUGUI>().fontSize = 26;
            }
        }
    }


    private string UniformKeybindNameForInGameUI(string _behaveKeybind_InUI)
    {
        if (_behaveKeybind_InUI.StartsWith("Alpha"))
        {
            _behaveKeybind_InUI = _behaveKeybind_InUI.Remove(0, 5);
        }

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            if (_behaveKeybind_InUI.Equals("Mouse0"))
            {
                _behaveKeybind_InUI = "LMB";
            }

            if (_behaveKeybind_InUI.Equals("Mouse1"))
            {
                _behaveKeybind_InUI = "RMB";
            }

            if (_behaveKeybind_InUI.StartsWith("Left"))
            {
                _behaveKeybind_InUI = _behaveKeybind_InUI.Remove(1, 3);
            }

            _behaveKeybind_InUI = _behaveKeybind_InUI.ToUpper();
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            if (_behaveKeybind_InUI.Equals("Mouse0"))
            {
                _behaveKeybind_InUI = "Êó±ê×ó¼ü";
            }

            if (_behaveKeybind_InUI.Equals("Mouse1"))
            {
                _behaveKeybind_InUI = "Êó±êÓÒ¼ü";
            }

            if (_behaveKeybind_InUI.StartsWith("Left"))
            {
                _behaveKeybind_InUI = _behaveKeybind_InUI.Remove(0, 4);
                _behaveKeybind_InUI = _behaveKeybind_InUI.Insert(0, "×ó");
            }
        }

        return _behaveKeybind_InUI;
    }
}
