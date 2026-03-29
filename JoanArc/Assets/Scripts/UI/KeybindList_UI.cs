using System.Collections.Generic;
using UnityEngine;

public class KeybindList_UI : MonoBehaviour
{
    [SerializeField] private GameObject keybindOptionPrefab;

    public List<GameObject> keybindOptionList { get; private set; } = new List<GameObject>();

    private void Start()
    {
        GenerateKeybindList();
    }

    private void GenerateKeybindList()
    {
        foreach (var search in KeyBindManager.instance.keybindsDictionary)
        {
            if (search.Key != null)
            {
                GameObject keybindOption = Instantiate(keybindOptionPrefab, transform);
                keybindOption.GetComponent<KeybindOptionController>().SetupKeybindOption(search.Key, search.Value.ToString());

                keybindOptionList.Add(keybindOption);
            }
        }
    }

    public void UpdateAllKeybindOptionsLanguage()
    {
        if (keybindOptionList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < keybindOptionList.Count; i++)
        {
            keybindOptionList[i].GetComponent<KeybindOptionController>()?.TranslateBehaveNameAndUniformBehaveKeybindName();
        }
    }
}
