using TMPro;
using UnityEngine;

public class KeybindConflictController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName_InUI;
    [SerializeField] protected TextMeshProUGUI behaveKeybind_InUI;

    private string behaveName;
    private string behaveKeybind;

    public void SetupKeybindConflict(string _behaveName, string _behaveKeybind)
    {
        behaveName = _behaveName;
        behaveKeybind = _behaveKeybind;

        behaveName_InUI.text = _behaveName;
        behaveKeybind_InUI.text = _behaveKeybind;

        TranslateBehaveNameAndUniformBehaveKeybindName();

        //behaveKeybind_InUI.text = UniformKeybindName(_behaveKeybind);
    }

    public void TranslateBehaveNameAndUniformBehaveKeybindName()
    {
        //chinese
        if (LanguageManager.instance.localeID == 1)
        {
            behaveName_InUI.text = LanguageManager.instance.EnglishToChineseKeybindsDictionary[behaveName];
        }
        //english
        else if (LanguageManager.instance.localeID == 0)
        {
            behaveName_InUI.text = behaveName;
        }

        //uniform keybind will auto detect language and translate
        behaveKeybind_InUI.text = UniformKeybindName(behaveKeybind);
    }

    private string UniformKeybindName(string _behaveKeybind_InUI)
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
                _behaveKeybind_InUI = "Mouse Left";
            }

            if (_behaveKeybind_InUI.Equals("Mouse1"))
            {
                _behaveKeybind_InUI = "Mouse Right";
            }

            if (_behaveKeybind_InUI.StartsWith("Left"))
            {
                _behaveKeybind_InUI = _behaveKeybind_InUI.Insert(4, " ");
            }
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
