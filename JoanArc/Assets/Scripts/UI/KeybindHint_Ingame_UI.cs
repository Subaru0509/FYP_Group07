using TMPro;
using UnityEngine;

public class KeybindHint_Ingame_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keybindText;
    [SerializeField] private string keybindTextPreset;
    [SerializeField] private string behaveName;


    private void Start()
    {
        GetKeybindIfAvailable();
    }

    private void OnEnable()
    {
        GetKeybindIfAvailable();
    }

    private void GetKeybindIfAvailable()
    {
        if (KeyBindManager.instance == null)
        {
            return;
        }

        if (KeyBindManager.instance.keybindsDictionary.ContainsKey(behaveName))
        {
            keybindText.text = UniformKeybindName(KeyBindManager.instance.keybindsDictionary[behaveName].ToString());
            keybindText.text = keybindTextPreset + keybindText.text;
        }
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
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            if (_behaveKeybind_InUI.Equals("Mouse0"))
            {
                _behaveKeybind_InUI = "×ó¼ü";
            }

            if (_behaveKeybind_InUI.Equals("Mouse1"))
            {
                _behaveKeybind_InUI = "ÓÒ¼ü";
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
