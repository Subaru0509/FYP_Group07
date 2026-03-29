using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [Header("Blackhole Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI blackholeUnlockButton;
    public bool blackholeUnlocked {  get; private set; }

    [SerializeField] private GameObject blackholePrefab;
    [Space]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int cloneAttackAmount;
    [SerializeField] private float cloneAttackCooldown;
    [Space]
    [SerializeField] private float QTEInputWindow;

    private GameObject currentBlackhole;
    private BlackholeSkillController currentBlackholeScript;

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        currentBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity); ;

        currentBlackholeScript = currentBlackhole.GetComponent<BlackholeSkillController>();

        currentBlackholeScript.SetupBlackholeSkill(maxSize, growSpeed, shrinkSpeed, cloneAttackAmount, cloneAttackCooldown, QTEInputWindow);

        AudioManager.instance.PlaySFX(3, player.transform);
        AudioManager.instance.PlaySFX(6, player.transform);
    }

    public override bool UseSkillIfAvailable()
    {
        return base.UseSkillIfAvailable();
    }

    public bool CanExitBlackholeSkill()
    {
        if (currentBlackholeScript == null)
        {
            return false;
        }

        if (currentBlackholeScript.CloneAttackHasFinished())
        {
            currentBlackholeScript = null;
            return true;
        }

        return false;
    }

    protected override void CheckUnlockFromSave()
    {
        UnlockBlackhole();
    }

    #region Unlock Skill
    private void UnlockBlackhole()
    {
        if (blackholeUnlocked)
        {
            return;
        }

        if (blackholeUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
        }
    }
    #endregion

}
