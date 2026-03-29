using System.Collections.Generic;
using UnityEngine;

public class KeybindConflictPromptWindowController : MonoBehaviour
{
    [SerializeField] private GameObject[] keybindConflicts;

    //will consider at most 2 conflict keybinds
    public void SetupKeybindConflictPromptWindow(KeyCode _keyCode)
    {
        List<string> _behaveNames = new List<string>();

        foreach (var search in KeyBindManager.instance.keybindsDictionary)
        {
            if (search.Value == _keyCode)
            {
                //behaveName here is always english
                _behaveNames.Add(search.Key);
            }
        }


        if (_behaveNames.Count >= 2)
        {
            //KeybindConflictController[] controllers = GetComponentsInChildren<KeybindConflictController>();

            //will consider at most 2 conflict keybinds
            int k = 0;

            for (int i = 0; i < keybindConflicts.Length; i++)
            {
                keybindConflicts[i].GetComponent<KeybindConflictController>()?.SetupKeybindConflict(_behaveNames[k], _keyCode.ToString());
                k++;
                //Debug.Log("Keybind conflict unit has been set up");

                if (k == 2)
                {
                    return;
                }
            }

        }
    }

    public void ClosePromptWindow()
    {
        Destroy(gameObject);
    }
}
