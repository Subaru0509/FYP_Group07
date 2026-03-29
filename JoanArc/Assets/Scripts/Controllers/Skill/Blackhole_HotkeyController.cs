using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotkeyController : MonoBehaviour
{
    private KeyCode hotkey;
    private TextMeshProUGUI textMesh;
    private SpriteRenderer sr;

    private Transform enemyTransform;
    private BlackholeSkillController blackholeScript;

    public void SetupHotkey(KeyCode _hotkey, Transform _enemyTransform, BlackholeSkillController _blackholeScript)
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
        enemyTransform = _enemyTransform;
        blackholeScript = _blackholeScript;
        hotkey = _hotkey;

        textMesh.text = hotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotkey))
        {
            blackholeScript.AddEnemyToList(enemyTransform);

            textMesh.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
