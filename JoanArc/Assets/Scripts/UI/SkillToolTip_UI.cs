using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillToolTip_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillPrice;

    public void ShowToolTip(string _skillName, string _skillDescription, string _skillPrice)
    {
        skillName.text = _skillName;
        skillDescription.text = _skillDescription;

        if (LanguageManager.instance.localeID == 0)
        {
            skillPrice.text = $"Skill price: {_skillPrice}";
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            skillPrice.text = $"技能价格: {_skillPrice}";
        }

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
