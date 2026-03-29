using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    [TextArea]
    [SerializeField] private string statDescription_Chinese;

    private void OnValidate()
    {
        gameObject.name = $"Stat - {statName}";

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    private void Start()
    {
        UpdateStatValue_UI();
    }

    public void UpdateStatValue_UI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStatByType(statType).GetValue().ToString();

            switch (statType)
            {
                case StatType.maxHP:
                    statValueText.text = playerStats.getMaxHP().ToString();
                    break;
                case StatType.damage:
                    statValueText.text = playerStats.GetDamage().ToString();
                    break;
                case StatType.critPower:
                    statValueText.text = playerStats.GetCritPower().ToString();
                    break;
                case StatType.critChance:
                    statValueText.text = playerStats.GetCritChance().ToString();
                    break;
                case StatType.evasion:
                    statValueText.text = playerStats.GetEvasion().ToString();
                    break;
                case StatType.magicResistance:
                    statValueText.text = playerStats.GetMagicResistance().ToString();
                    break;

                case StatType.fireDamage:
                    statValueText.text = playerStats.GetFireDamage().ToString();
                    break;

                case StatType.iceDamage:
                    statValueText.text = playerStats.GetIceDamage().ToString();
                    break;

                case StatType.lightningDamage:
                    statValueText.text = playerStats.GetLightningDamage().ToString();
                    break;
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 offset = ui.SetupToolTipPositionOffsetAccordingToUISlotPosition(transform, 0.1f, 0.1f, 0.18f, 0.04f);
        ui.statToolTip.transform.position = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            ui.statToolTip.ShowStatToolTip(statDescription);

        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            ui.statToolTip.ShowStatToolTip(statDescription_Chinese);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
