using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindManager : MonoBehaviour, ISettingsSaveManager
{
    public static KeyBindManager instance;

    public Dictionary<string, KeyCode> keybindsDictionary;

    [SerializeField] private KeybindList_UI keybindList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        keybindsDictionary = new Dictionary<string, KeyCode>();
    }

    private void Update()
    {

    }
    
    public void UpdateKeybindListLanguage()
    {
        keybindList.UpdateAllKeybindOptionsLanguage();
    }

    //public string GetKeyCodeByBehaveName(string _behaveName)
    //{
    //    if (keybindsDictionary.TryGetValue(_behaveName, out KeyCode key))
    //    {
    //        return $"The keybind for this behave is: {key.ToString()}";
    //    }
    //    return "Didn't find keybind for this behave";
    //}

    //public void GetAllKeyBinds()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        KeyBind keybind = transform.GetChild(i).GetComponent<KeyBind>();

    //        if (keybind != null)
    //        {
    //            keyBindDictionary.Add(keybind.behaveName, keybind.behaveKeyBind);
    //        }
    //    }
    //}

    public string UniformKeybindName(string _behaveKeybind_InUI)
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


    public void LoadData(SettingsData _data)
    {
        foreach (var search in _data.keybindsDictionary)
        {
            keybindsDictionary.Add(search.Key, search.Value);
        }
    }

    public void SaveData(ref SettingsData _data)
    {
        _data.keybindsDictionary.Clear();

        foreach (var search in keybindsDictionary)
        {
            _data.keybindsDictionary.Add(search.Key, search.Value);
        }
    }
}
