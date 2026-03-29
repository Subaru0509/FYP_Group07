using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    private TextMeshPro tutorialText;

    [SerializeField] private List<string> boundBehaveNameList;
    [TextArea][SerializeField] private string originalText;
    [TextArea][SerializeField] private string originalText_Chinese;

    private void Awake()
    {
        tutorialText = GetComponent<TextMeshPro>();
    }


    private void Update()
    {
        UpdateTutorialText();
    }

    private void UpdateTutorialText()
    {
        string completedOriginalText = AddBehaveKeybindNameToTutorialText(originalText);
        string completedOriginalText_Chinese = AddBehaveKeybindNameToTutorialText(originalText_Chinese);

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            tutorialText.text = completedOriginalText;
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            tutorialText.text = completedOriginalText_Chinese;
        }
    }

    private string AddBehaveKeybindNameToTutorialText(string _tutorialText)
    {
        for (int i = 0; i < boundBehaveNameList.Count; i++)
        {
            if (_tutorialText.Contains($"BehaveName{i}"))
            {
                string _keybindName = KeyBindManager.instance.keybindsDictionary[boundBehaveNameList[i]].ToString();
                _keybindName = KeyBindManager.instance.UniformKeybindName(_keybindName);
                _tutorialText = _tutorialText.Replace($"BehaveName{i}", _keybindName);
            }
        }

        return _tutorialText;
    }
}
