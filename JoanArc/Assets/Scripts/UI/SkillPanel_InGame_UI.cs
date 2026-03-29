using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel_InGame_UI : MonoBehaviour
{
    public static SkillPanel_InGame_UI instance;

    public GameObject dashIcon;
    public GameObject parryIcon;
    public GameObject crystalIcon;
    public GameObject throwSwordIcon;
    public GameObject blackholeIcon;

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
    }

    private void OnEnable()
    {
        ShowAllSkillIconsAccordingToUnlockState();
    }

    private void Start()
    {
        //hide all skill icons first to make sure skill panel can show all the unlocked skills correctly
        //becuase SkillPanel script loads faster than SkillTreeSlot_UI script
        HideAllSkillIcons();
        ShowAllSkillIconsAccordingToUnlockState();
        UpdateAllSkillIconTexts();
    }

    public void UpdateAllSkillIconTexts()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform?.GetChild(i)?.GetComponentInChildren<SkillIconText_InGame_UI>()?.UpdateSkillIconText();
        }
    }

    private void HideAllSkillIcons()
    {
        dashIcon.SetActive(false);
        parryIcon.SetActive(false);
        crystalIcon.SetActive(false);
        throwSwordIcon.SetActive(false);
        blackholeIcon.SetActive(false);
    }

    public void ShowAllSkillIconsAccordingToUnlockState()
    {
        if (SkillManager.instance.dash.dashUnlocked)
        {
            dashIcon.SetActive(true);
        }
        else
        {
            dashIcon.SetActive(false);
        }

        if(SkillManager.instance.parry.parryUnlocked)
        {
            parryIcon.SetActive(true);
        }
        else
        {
            parryIcon.SetActive(false);
        }

        if (SkillManager.instance.crystal.crystalUnlocked)
        {
            crystalIcon.SetActive(true);
        }
        else
        {
            crystalIcon.SetActive(false);
        }

        if (SkillManager.instance.sword.throwSwordSkillUnlocked)
        {
            throwSwordIcon.SetActive(true);
        }
        else
        {
            throwSwordIcon.SetActive(false);
        }

        if (SkillManager.instance.blackhole.blackholeUnlocked)
        {
            blackholeIcon.SetActive(true);
        }
        else
        {
            blackholeIcon.SetActive(false);
        }
    }
}
