using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [Space]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalExistenceDuration;
    private GameObject currentCrystal;

    [Header("Crystal Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI crystalUnlockButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Mirage Blink Unlock Info")]  //spawn clone on original position when teleporting to crystal position
    [SerializeField] private SkillTreeSlot_UI mirageBlinkUnlockButton;
    public bool mirageBlinkUnlocked { get; private set; }

    [Header("Explosive Crystal Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI explosiveCrystalUnlockButton;
    public bool explosiveCrystalUnlocked { get; private set; }

    [Header("Moving Crystal Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI movingCrystalUnlockButton;
    public bool movingCrystalUnlocked { get; private set; }
    [SerializeField] private float moveSpeed;

    [Header("Crystal Gun Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI crystalGunUnlockButton;
    public bool crystalGunUnlocked { get; private set; }
    [SerializeField] private int magSize;
    [SerializeField] private float shootCooldown;  //represents crystal gun's fire rate
    [SerializeField] private float reloadTime;
    [SerializeField] private float shootWindow;
    private float shootWindowTimer;
    [SerializeField] private List<GameObject> crystalMag = new List<GameObject>();
    private bool reloading = false;


    protected override void Start()
    {
        base.Start();

        crystalUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockCrystal);
        mirageBlinkUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockCrystalMirage);
        explosiveCrystalUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockExplosiveCrystal);
        movingCrystalUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockMovingCrystal);
        crystalGunUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockCrystalGun);
    }

    protected override void Update()
    {
        base.Update();

        shootWindowTimer -= Time.deltaTime;

        //if haven't shoot all the ammo in the mag for a while
        //auto reload the mag
        //shootWindowTimer = shootWindow; in ShootCrystalGunIfAvailable()
        //add !reloading to prevent calling coroutine multiple times
        //when using invoke in the past, the invoke is gonna get called lots of times
        //because there's gonna be much time in the shootWindowTimer <= 0 && 0 < ammo < magsize state
        if (shootWindowTimer <= 0 && crystalMag.Count > 0 && crystalMag.Count < magSize && !reloading)
        {
            ReloadCrystalMag();
        }
    }

    protected override void CheckUnlockFromSave()
    {
        UnlockCrystal();
        UnlockCrystalGun();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
    }

    public override bool UseSkillIfAvailable()
    {
        if (crystalGunUnlocked)
        {
            UseSkill();
            return true;
        }
        else
        {
            if (cooldownTimer < 0)
            {
                UseSkill();
                return true;
            }

            //english
            if (LanguageManager.instance.localeID == 0)
            {
                player.fx.CreatePopUpText("Skill is in cooldown");

            }
            //chinese
            else if (LanguageManager.instance.localeID == 1)
            {
                player.fx.CreatePopUpText("技能冷却中！");
            }

            return false;
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();

        //if in crystal gun mode, disabling all single crystal functions
        if (crystalGunUnlocked)
        {
            if (ShootCrystalGunIfAvailable())
            {
                //update the crystal skill UI Icon in skill panel
                EnterCooldown();
                return;
            }

            //english
            if (LanguageManager.instance.localeID == 0)
            {
                player.fx.CreatePopUpText("Skill is in cooldown");

            }
            //chinese
            else if (LanguageManager.instance.localeID == 1)
            {
                player.fx.CreatePopUpText("技能冷却中！");
            }

            return;
        }

        //if there's no crystal yet, create crystal
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            //if crystal can move towards enemy,
            //teleport function will be disabled
            if (movingCrystalUnlocked)
            {
                return;
            }

            //spawn clone then teleport
            //*****************************************************************
            //***Cannot enable this when Replace Clone By Crystal is enabled***
            //*****************************************************************
            if (mirageBlinkUnlocked)
            {
                SkillManager.instance.clone.CreateClone(player.transform.position);
                //Destroy(currentCrystal);
                currentCrystal.GetComponent<CrystalSkillController>()?.crystalSelfDestroy();
            }

            //player teleport to the crystal's position
            Vector2 playerPosition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            currentCrystal.GetComponent<CrystalSkillController>()?.EndCrystal_ExplodeIfAvailable();

            EnterCooldown();
        }
    }

    public void CreateCrystal()
    {
        //create a crystal
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity); ;
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(crystalExistenceDuration, explosiveCrystalUnlocked, movingCrystalUnlocked, moveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    private void ReloadCrystalMag()
    {
        if (reloading)
        {
            return;
        }

        StartCoroutine(ReloadCrystalMag_Coroutine());
    }

    private IEnumerator ReloadCrystalMag_Coroutine()
    {
        reloading = true;
        EnterCooldown();

        yield return new WaitForSeconds(reloadTime);

        Reload();
        reloading = false;
    }

    private void Reload()
    {
        if (!reloading)
        {
            return;
        }

        int ammoToAdd = magSize - crystalMag.Count;

        for (int i = 0; i < ammoToAdd; i++)
        {
            crystalMag.Add(crystalPrefab);
        }
    }

    private bool ShootCrystalGunIfAvailable()
    {
        if (crystalGunUnlocked && !reloading)
        {
            if (crystalMag.Count > 0)
            {
                GameObject crystalToSpawn = crystalMag[crystalMag.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity); ;

                crystalMag.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>()?.
                    SetupCrystal(crystalExistenceDuration, explosiveCrystalUnlocked, movingCrystalUnlocked, moveSpeed, FindClosestEnemy(newCrystal.transform));


                shootWindowTimer = shootWindow;

                if (crystalMag.Count <= 0)
                {
                    ReloadCrystalMag();
                }
            }

            return true;
        }

        return false;
    }

    public void DestroyCurrentCrystal_InCrystalMirageOnly()
    {
        if (currentCrystal != null)
        {
            Destroy(currentCrystal);
        }
    }

    public void CurrentCrystalSpecifyEnemy(Transform _enemy)
    {
        if (currentCrystal != null)
        {
            currentCrystal.GetComponent<CrystalSkillController>()?.SpecifyEnemyTarget(_enemy);
        }
    }

    public void EnterCooldown()
    {
        if (cooldownTimer < 0 && !crystalGunUnlocked)
        {
            InGame_UI.instance.SetCrystalCooldownImage();
            cooldownTimer = cooldown;
        }
        else if (cooldownTimer < 0 && crystalGunUnlocked)
        {
            InGame_UI.instance.SetCrystalCooldownImage();
            cooldownTimer = GetCrystalCooldown();
        }

        skillLastUseTime = Time.time;
    }

    public float GetCrystalCooldown()
    {
        //if crystal gun is not unlocked, cooldown should be the default cooldown
        float crystalCooldown = cooldown;

        //if crystal gun is unlocked
        if (crystalGunUnlocked)
        {
            if (shootWindowTimer > 0)
            {
                //when shootable, cooldown should be the fire interval
                crystalCooldown = shootCooldown;
            }

            if (reloading)
            {
                //when reloading, cooldown should be the crystal gun reload time
                crystalCooldown = reloadTime;
            }
        }

        return crystalCooldown;
    }

    public override bool SkillIsReadyToUse()
    {
        if (crystalGunUnlocked)
        {
            if (!reloading)
            {
                return true;
            }
            return false;
        }
        else
        {
            if (cooldownTimer < 0)
            {
                return true;
            }
            return false;
        }
    }

    //public Transform GetCurrentCrystalTransform()
    //{
    //    return currentCrystal?.transform;
    //}


    //public void CurrentCrystalChooseRandomEnemy(float _searchRadius)
    //{
    //    if (currentCrystal != null)
    //    {
    //        currentCrystal.GetComponent<CrystalSkillController>()?.CrystalChooseRandomEnemy(_searchRadius);
    //    }
    //}

    #region Unlock Crystal Skills
    private void UnlockCrystal()
    {
        if (crystalUnlocked)
        {
            return;
        }

        if (crystalUnlockButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    private void UnlockCrystalMirage()
    {
        if (mirageBlinkUnlocked)
        {
            return;
        }

        if (mirageBlinkUnlockButton.unlocked)
        {
            mirageBlinkUnlocked = true;
        }
    }

    private void UnlockExplosiveCrystal()
    {
        if (explosiveCrystalUnlocked)
        {
            return;
        }

        if (explosiveCrystalUnlockButton.unlocked)
        {
            explosiveCrystalUnlocked = true;
        }
    }

    private void UnlockMovingCrystal()
    {
        if (movingCrystalUnlocked)
        {
            return;
        }

        if (movingCrystalUnlockButton.unlocked)
        {
            movingCrystalUnlocked = true;
        }
    }

    private void UnlockCrystalGun()
    {
        if (crystalGunUnlocked)
        {
            return;
        }

        if (crystalGunUnlockButton.unlocked)
        {
            crystalGunUnlocked = true;
        }
    }
    #endregion
}
