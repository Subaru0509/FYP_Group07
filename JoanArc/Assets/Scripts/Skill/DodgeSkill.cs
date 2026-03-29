using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI dodgeUnlockButton;
    [SerializeField] private int evasionIncreasement;
    public bool dodgeUnlocked;

    [Header("Mirade Dodge")]
    [SerializeField] private SkillTreeSlot_UI mirageDodgeUnlockButton;
    public bool mirageDodgeUnlocked;

    protected override void Start()
    {
        base.Start();

        dodgeUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockDodge);
        mirageDodgeUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockMirageDodge);
    }


    public void CreateMirageOnDodge()
    {
        if (mirageDodgeUnlocked)
        {
            SkillManager.instance.clone.CreateClone(new Vector3(player.transform.position.x + 2 * player.facingDirection, player.transform.position.y));
        }
    }

    protected override void CheckUnlockFromSave()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    #region Unlock Skill
    private void UnlockDodge()
    {
        if (dodgeUnlocked)
        {
            return;
        }

        if (dodgeUnlockButton.unlocked)
        {
            dodgeUnlocked = true;
            player.stats.evasion.AddModifier(evasionIncreasement);
            //update the player stat UI immediately after unlocking dodge skill
            Inventory.instance.UpdateStatUI();
        }
    }
    
    private void UnlockMirageDodge()
    {
        if (mirageDodgeUnlocked)
        {
            return;
        }

        if (mirageDodgeUnlockButton.unlocked)
        {
            mirageDodgeUnlocked = true;
        }
    }
    #endregion
}
