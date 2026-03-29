using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    public static InGame_UI instance;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image throwSwordImage;
    [SerializeField] private Image blackholeImage;

    //[SerializeField] private Image flaskImage;

    [Header("Souls Info")]
    [SerializeField] private TextMeshProUGUI currentCurrency;
    [SerializeField] private float currencyAmount;
    [SerializeField] private float increaseRate = 100;
    [SerializeField] private float defaultcurrencyFontSize;
    [SerializeField] private float currencyFontSizeWhenIncreasing;

    private SkillManager skill;
    private Player player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHPUI;
        }

        skill = SkillManager.instance;
        player = PlayerManager.instance.player;
        UpdateHPUI();
    }

    private void Update()
    {
        //for number increasing animations when loading or getting currency
        UpdateCurrencyUI();

        if (Input.GetKeyDown(/*KeyCode.LeftShift*/ KeyBindManager.instance.keybindsDictionary["Dash"]) && skill.dash.dashUnlocked)
        {
            SetSkillCooldownImage(dashImage);
        }

        if (Input.GetKeyDown(/*KeyCode.Q*/ KeyBindManager.instance.keybindsDictionary["Parry"]) && skill.parry.parryUnlocked)
        {
            SetSkillCooldownImage(parryImage);
        }


        //crystal cooldown image is set in CrystalSkillController 
        //if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
        //{
        //    SetSkillCooldownImage(crystalImage);
        //}

        if (Input.GetKeyDown(/*KeyCode.Mouse1*/ KeyBindManager.instance.keybindsDictionary["Aim"]) && skill.sword.throwSwordSkillUnlocked)
        {
            SetSkillCooldownImage(throwSwordImage);
        }

        if (Input.GetKeyDown(/*KeyCode.R*/ KeyBindManager.instance.keybindsDictionary["Blackhole"]) && skill.blackhole.blackholeUnlocked)
        {
            SetSkillCooldownImage(blackholeImage);
        }

        //There's no SetFlaskCooldownImage() function because FillFlaskCooldownImage is in a different logic
        //and it'll do the same stuff on different kinds of flasks
        //CDs for different flasks are calculated seperately

        FillSkillCooldownImage(dashImage, skill.dash.cooldown);
        FillSkillCooldownImage(parryImage, skill.parry.cooldown);
        FillSkillCooldownImage(crystalImage, skill.crystal.GetCrystalCooldown());
        FillSkillCooldownImage(throwSwordImage, skill.sword.cooldown);
        FillSkillCooldownImage(blackholeImage, skill.blackhole.cooldown);
        FillFlaskCooldownImage();
    }

    private void UpdateCurrencyUI()
    {
        if (currencyAmount < PlayerManager.instance.GetCurrentCurrency())
        {
            IncraseCurrencyFontSize();
            currencyAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            currencyAmount = PlayerManager.instance.GetCurrentCurrency();
            DecreaseCurrencyFontSizeToDefault();
        }


        if (currencyAmount == 0)
        {
            currentCurrency.text = currencyAmount.ToString();
        }
        else
        {
            currentCurrency.text = currencyAmount.ToString("#,#");
        }
    }

    private void IncraseCurrencyFontSize()
    {
        currentCurrency.fontSize = Mathf.Lerp(currentCurrency.fontSize, currencyFontSizeWhenIncreasing, 100 * Time.deltaTime);
    }

    private void DecreaseCurrencyFontSizeToDefault()
    {
        currentCurrency.fontSize = Mathf.Lerp(currentCurrency.fontSize, defaultcurrencyFontSize, 5 * Time.deltaTime);
    }

    private void UpdateHPUI()
    {
        //total_maxHP = maxHP + vitality * 5
        slider.maxValue = playerStats.getMaxHP();
        slider.value = playerStats.currentHP;
    }

    private void SetSkillCooldownImage(Image _skillImage)
    {
        //0 means skill is ready to use, default color
        if (_skillImage.fillAmount <= 0)
        {
            //1 means skill is in cooldown, darker color
            _skillImage.fillAmount = 1;
        }
    }

    public void SetCrystalCooldownImage()
    {
        SetSkillCooldownImage(crystalImage);
    }

    private void FillSkillCooldownImage(Image _skillImage, float _cooldown)
    {
        if (_skillImage == null)
        {
            return;
        }

        if (_skillImage.fillAmount > 0)
        {
            //the percentage increasement of the cooldown progression as time goes by
            //1 means in cooldown, 0 means ready to use
            _skillImage.fillAmount -= (1 / _cooldown) * Time.deltaTime;
        }
    }

    private void FillFlaskCooldownImage()
    {
        Image _flaskImage = Flask_UI.instance.flaskCooldownImage;

        if (_flaskImage == null)
        {
            return;
        }

        //each flask should be ready to use in the beginning
        ItemData_Equipment flask = Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Flask);

        if (flask == null)
        {
            return;
        }

        if (!flask.itemEffects[0].effectUsed)
        {
            _flaskImage.fillAmount = 0;
            return;
        }

        if (_flaskImage.fillAmount >= 0)
        {
            float timer = Time.time - flask.itemEffects[0].effectLastUseTime;
            _flaskImage.fillAmount = 1 - ((1 / flask.itemEffects[0].effectCooldown) * timer);
        }
    }
}
