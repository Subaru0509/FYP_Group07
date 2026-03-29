using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeybindOptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName_InUI;
    [SerializeField] protected TextMeshProUGUI behaveKeybind_InUI;
    [SerializeField] private Button keybindButton;
    [Space]
    [SerializeField] private GameObject keybindConflictPromptWindowPrefab;

    //all these 2 are english
    private string behaveName;
    private string behaveKeybind;

    private void Start()
    {
        keybindButton.onClick.AddListener(ChangeKeybind);
    }

    //_behaveName is english, cuz KeybindManager.instance.keybindsDictionary only supports english
    public void SetupKeybindOption(string _behaveName, string _behaveKeybind)
    {
        behaveName = _behaveName;
        behaveKeybind = _behaveKeybind;

        behaveName_InUI.text = _behaveName;
        behaveKeybind_InUI.text = _behaveKeybind;

        TranslateBehaveNameAndUniformBehaveKeybindName();
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

        //UniformKeybindName will auto detect language and translate
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

    public void ChangeKeybind()
    {
        StartCoroutine(ChangeKeybindInput());

        //show awaiting input prompt window UI
        //Debug.Log($"awaiting input for {behaveName.text}");
        //behaveKeybind.text = "Awaiting input: ";

        //get user keycode input and change the keybind text in UI
        //StartCoroutine(CheckInput_Coroutine());

        //update the keybind dictionary
    }

    private IEnumerator ChangeKeybindInput()
    {
        //show awaiting input prompt window UI
        if (LanguageManager.instance.localeID == 0)
        {
            behaveKeybind_InUI.text = "Awaiting input";
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            behaveKeybind_InUI.text = "µÈ´ýÊäÈë°´¼ü";
        }

        UI.instance?.EnableUIKeyInput(false);
        MainMenu_UI.instance?.EnableKeyInput(false);

        yield return new WaitUntil(CheckInput);

        keybindButton.onClick.RemoveAllListeners();

        //unity has a bug, have to do null check like this here
        if (SkillPanel_InGame_UI.instance != null)
        {
            SkillPanel_InGame_UI.instance?.UpdateAllSkillIconTexts();
        }

        yield return new WaitWhile(HasAnyKey);

        keybindButton.interactable = true;
        keybindButton.onClick.AddListener(ChangeKeybind);

        UI.instance?.EnableUIKeyInput(true);
        MainMenu_UI.instance?.EnableKeyInput(true);
    }

    private bool CheckInput()
    {
        keybindButton.interactable = false;

        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                if (keycode == KeyCode.Escape)
                {
                    behaveKeybind_InUI.text = UniformKeybindName(KeyBindManager.instance.keybindsDictionary[behaveName].ToString());
                    Debug.Log("Keybind change cancelled");
                    return true;
                }

                behaveKeybind_InUI.text = UniformKeybindName(keycode.ToString());
                behaveKeybind = keycode.ToString();
                Debug.Log($"{behaveName_InUI.text} keybind has changed to {keycode.ToString()}");

                KeyBindManager.instance.keybindsDictionary[behaveName] = keycode;

                if (HasKeybindConflict(keycode))
                {
                    //parent is Keybind_Options
                    //instantiate keybind conflict prompt window
                    GameObject newKeybindConflictPromptWindow = Instantiate(keybindConflictPromptWindowPrefab, transform.parent.parent.parent.parent);

                    //setup keybind conflict prompt window
                    newKeybindConflictPromptWindow.GetComponent<KeybindConflictPromptWindowController>()?.SetupKeybindConflictPromptWindow(keycode);
                }

                return true;
            }

        }

        //Debug.Log("keybind change failed");
        return false;
    }


    private bool HasAnyKey()
    {
        if (Input.anyKey || Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    private bool HasKeybindConflict(KeyCode _keycode)
    {
        foreach (var search in KeyBindManager.instance.keybindsDictionary)
        {
            //will not conflict with itself
            if (search.Key == behaveName)
            {
                continue;
            }

            if (search.Value == _keycode)
            {
                return true;
            }
        }

        return false;
    }

}
