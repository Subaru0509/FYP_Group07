using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    //assign different kinds of clones' attack damage multiplier to this variable
    private float currentCloneAttackDamageMultipler;

    //clone == mirage
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLosingSpeed;


    [Header("Mirage Attack Unlock Info")] //unlock the clone ability, clone will attack enemy as the default ability
    [SerializeField] private SkillTreeSlot_UI mirageAttackUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float cloneAttackDamageMultiplier;  //clone attack damage should be less than player's damage
    public bool mirageAttackUnlocked { get; private set; }


    [Header("Aggressive Mirage Unlock Info")] //aggressive mirage will make clone do more damage and able to apply on-hit effects
    [SerializeField] private SkillTreeSlot_UI aggressiveMirageUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float aggressiveCloneAttackDamageMultiplier;
    public bool aggressiveMirageUnlocked { get; private set; }
    public bool aggressiveCloneCanApplyOnHitEffect { get; private set; }


    [Header("Multiple Mirage Unlock Info")] //clone can create clone
    [SerializeField] private SkillTreeSlot_UI multipleMirageUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float duplicateCloneAttackDamageMultiplier;  //duplicate clone deals 30% damage of player
    public bool multipleMirageUnlocked { get; private set; }
    [SerializeField] private float duplicatePossibility;
    public int maxDuplicateCloneAmount; //prevent creating endless duplicate clones
    [HideInInspector] public int currentDuplicateCloneAmount;


    [Header("Crystal Mirage Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI crystalMirageUnlockButton;
    public bool crystalMirageUnlocked { get; private set; }


    protected override void Start()
    {
        base.Start();

        mirageAttackUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockMirageAttack);
        aggressiveMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockAggressiveMirage);
        multipleMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockMultipleMirage);
        crystalMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockCrystalMirage);
    }


    //prevent creating endless duplicate clones
    public void RefreshCurrentDuplicateCloneAmount()
    {
        currentDuplicateCloneAmount = 0;
    }

    public void CreateClone(Vector3 _position)
    {
        //if replace clone by crstal is enabled,
        //will not create clones anymore
        //**************************************************************************
        //***Cannot enable Replace Clone By Crystal when Clone Mirage is enabled***
        //**************************************************************************
        if (crystalMirageUnlocked)
        {
            //if (SkillManager.instance.crystal.crystalMirageUnlocked)
            //{
            //    SkillManager.instance.crystal.crystalMirageUnlocked = false;
            //    Debug.Log("Clone_Mirage in Crystal Skill is DISABLED" +
            //        "\nBecase Replace_Clone_By_Crystal in Clone Skill is ENABLED");
            //}

            //prevent creating multiple crystals
            //SkillManager.instance.crystal.DestroyCurrentCrystal_InCrystalMirageOnly();

            if (SkillManager.instance.crystal.SkillIsReadyToUse())
            {
                SkillManager.instance.crystal.UseSkillIfAvailable();
            }
            return;
        }

        //prevent creating endless duplicate clones
        //or not being able to create duplicate clones
        RefreshCurrentDuplicateCloneAmount();

        GameObject newClone = Instantiate(clonePrefab, _position, Quaternion.identity);
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();

        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, mirageAttackUnlocked, FindClosestEnemy(newClone.transform), multipleMirageUnlocked, duplicatePossibility, currentCloneAttackDamageMultipler);
    }

    public void CreateDuplicateClone(Vector3 _position)
    {
        GameObject newClone = Instantiate(clonePrefab, _position, Quaternion.identity);
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();

        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, mirageAttackUnlocked, FindClosestEnemy(newClone.transform), multipleMirageUnlocked, duplicatePossibility, currentCloneAttackDamageMultipler);

        //prevent creating endless duplicate clones
        currentDuplicateCloneAmount++;
    }


    public void CreateCloneWithDelay(Vector3 _position, float _delay)
    {
        StartCoroutine(CreateCloneWithDelay_Coroutine(_position, _delay));
    }

    private IEnumerator CreateCloneWithDelay_Coroutine(Vector3 _position, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        CreateClone(_position);
    }


    protected override void CheckUnlockFromSave()
    {
        UnlockMirageAttack();
        UnlockAggressiveMirage();
        UnlockCrystalMirage();
        UnlockMultipleMirage();
    }

    #region Unlock Skill
    private void UnlockMirageAttack()
    {
        if (mirageAttackUnlocked)
        {
            return;
        }

        if (mirageAttackUnlockButton.unlocked)
        {
            mirageAttackUnlocked = true;
            currentCloneAttackDamageMultipler = cloneAttackDamageMultiplier;
        }
    }

    private void UnlockAggressiveMirage()
    {
        if (aggressiveMirageUnlocked)
        {
            return;
        }

        if (aggressiveMirageUnlockButton.unlocked)
        {
            aggressiveMirageUnlocked = true;
            aggressiveCloneCanApplyOnHitEffect = true;
            currentCloneAttackDamageMultipler = aggressiveCloneAttackDamageMultiplier;
        }
    }

    private void UnlockMultipleMirage()
    {
        if (multipleMirageUnlocked)
        {
            return;
        }

        if (multipleMirageUnlockButton.unlocked)
        {
            multipleMirageUnlocked = true;
            currentCloneAttackDamageMultipler = duplicateCloneAttackDamageMultiplier;
        }
    }

    private void UnlockCrystalMirage()
    {
        if (crystalMirageUnlocked)
        {
            return;
        }

        if (crystalMirageUnlockButton.unlocked)
        {
            crystalMirageUnlocked = true;
        }
    }
    #endregion
}
